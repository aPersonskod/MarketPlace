using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Models;
using Models.Dtos;
using Models.Extensions;
using Models.Interfaces;

namespace ShoppingCart.Services;

public class ShoppingCartService(
    DataContext dataContext,
    IProductCatalog productCatalog,
    UserClientService userService,
    IKafkaProducer<CartDto> kafkaCartProducer,
    ILogger<ShoppingCartService> logger,
    IDistributedCache cache) : IShoppingCart
{
    public Task<IEnumerable<CartDto>> GetCarts() => Task.FromResult(dataContext.ShoppingCarts.Select(GetCartDto));
    public Task<IEnumerable<PlaceDto>> GetPlaces() => Task.FromResult(dataContext.Places.Select(GetPlaceDto));
    public async Task<PlaceDto> GetPlace(Guid placeId)
    {
        var place = await dataContext.Places.FindAsync(placeId);
        if (place == null) throw new Exception("Place not found");
        return await Task.FromResult(GetPlaceDto(place));
    }

    public async Task<IEnumerable<OrderDto>> GetOrders(Guid cartId)
    {
        var cachedOrders = await cache.GetRecordAsync<List<Order>>(cartId.ToString());

        if (cachedOrders?.Count == 0)
        {
            cachedOrders = dataContext.Orders.Where(x => x.CartId == cartId && x.Quantity > 0).ToList();
            if (cachedOrders.Count > 0)
            {
                // сохранение в кэш на 10 минут
                await cache.SetRecordAsync(cartId.ToString(), cachedOrders, TimeSpan.FromMinutes(10));
            }
        }
        return cachedOrders?.Select(GetOrderDto) ?? [];
    }

    public async Task<CartDto> GetCart(string? accessToken = null)
    {
        var foundUserDto = await userService.GetUser(accessToken);
        if (foundUserDto == null) throw new Exception("User not found");
        var cart = await dataContext.ShoppingCarts.FirstOrDefaultAsync(c => c.UserId == foundUserDto.Id && !c.IsBought && !c.IsConfirmed);
        if (cart != null) return await Task.FromResult(GetCartDto(cart));
        var newCart = new Cart() { Id = Guid.NewGuid(), UserId = foundUserDto.Id };
        await dataContext.ShoppingCarts.AddAsync(newCart);
        await dataContext.SaveChangesAsync();
        return await Task.FromResult(GetCartDto(newCart));
    }

    public async Task<CartDto> GetCartById(Guid cartId)
    {
        var cart = await dataContext.ShoppingCarts.FindAsync(cartId);
        if (cart == null) throw new Exception("Cart not found");
        return await Task.FromResult(GetCartDto(cart));
    }

    public async Task<CartDto> AddOrder(Guid productId, int quantity, string? accessToken = null)
    {
        var cart = await GetCart(accessToken);
        logger.LogInformation($"cartId: {cart.Id}");
        logger.LogInformation($"orders count: {dataContext.Orders.Count()}");
        
        var orders = await cache.GetRecordAsync<List<Order>?>(cart.Id.ToString()) ?? [];
        var foundOrder = orders.FirstOrDefault(x => x.CartId == cart.Id && x.OrderedProductId == productId);
        if (foundOrder != null)
        {
            logger.LogInformation($"foundOrderId: {foundOrder.Id}");
            foundOrder.Quantity = quantity;
        }
        else
        {
            logger.LogInformation($"We try to create order, because they don't exist");
            var product = await productCatalog.Get(productId);
            logger.LogInformation($"found product: {product.Name}");
            var newOrder = new Order()
            {
                Id = Guid.NewGuid(),
                CartId = cart.Id,
                OrderedProductId = product.Id,
                Quantity = quantity
            };
            
            orders.Add(newOrder);
        }
        await cache.SetRecordAsync(cart.Id.ToString(), orders.Where(x => x.Quantity > 0), TimeSpan.FromMinutes(10));

        await ChangeAmountToPay(cart.Id);
        return await GetCart(accessToken);
    }

    public async Task<CartDto> DeleteOrder(Guid productId, string? accessToken = null)
    {
        var foundCart = await GetCart(accessToken);
        var cart = await dataContext.ShoppingCarts.FindAsync(foundCart.Id);
        var orders = await cache.GetRecordAsync<List<Order>?>(cart!.Id.ToString());
        var order = orders?.FirstOrDefault(x => x.OrderedProductId == productId);
        if (order == null) throw new Exception("Order not found when deleting");
        orders?.Remove(order);
        await cache.SetRecordAsync(cart.Id.ToString(), orders?.Where(x => x.Quantity > 0), TimeSpan.FromMinutes(10));
        await ChangeAmountToPay(cart!.Id);
        return await Task.FromResult(GetCartDto(cart));
    }

    [Obsolete("synchronous task that is too slow")]
    public async Task<CartDto> ConfirmCart(Guid placeId, string? accessToken = null)
    {
        var cart = await GetCart(accessToken);
        
        var isCartNotEmpty = await dataContext.Orders.AnyAsync(x => x.CartId == cart.Id);
        if (!isCartNotEmpty) throw new Exception($"Cart has no orders !!!");
        
        var foundUser = await userService.GetUser(accessToken);
        var isUserHasEnoughMoney = foundUser!.Wallet >= cart.AmountToPay;
        if (!isUserHasEnoughMoney) throw new Exception($"You have not enough money !!!");
        
        var foundCart = await dataContext.ShoppingCarts.FindAsync(cart.Id);
        foundCart!.IsConfirmed = true;
        await dataContext.SaveChangesAsync();
        return await Task.FromResult(GetCartDto(foundCart));
    }

    public async Task<CartDto> ConfirmAndBuyCart(Guid? placeId, string? accessToken = null)
    {
        var cart = await GetCart(accessToken);
        
        var orders = await cache.GetRecordAsync<List<Order>?>(cart.Id.ToString());
        var isCartNotEmpty = orders?.Any(x => x.CartId == cart.Id) ?? false;
        if (!isCartNotEmpty) throw new Exception($"Cart has no orders !!!");
        // todo very lazy code (used because redis)
        foreach (var order in orders!)
        {
            var foundOrder = await dataContext.Orders.FirstOrDefaultAsync(x => x.Id == order.Id);
            if (foundOrder == null)
            {
                await dataContext.Orders.AddAsync(order);
            }
            else
            {
                foundOrder.Quantity = order.Quantity;
            }
        }
        
        var foundUser = await userService.GetUser(accessToken);
        var isUserHasEnoughMoney = foundUser!.Wallet >= cart.AmountToPay;
        if (!isUserHasEnoughMoney) throw new Exception($"You have not enough money !!!");
        
        var foundCart = await dataContext.ShoppingCarts.FindAsync(cart.Id);
        if(foundCart == null) throw new Exception($"Undefined cart !!!");
        foundCart.PlaceId = placeId;
        if (foundCart.PlaceId == null) throw new Exception($"Cart's place is empty !!!");
        foundCart.IsConfirmed = true;
        await dataContext.SaveChangesAsync();
        await kafkaCartProducer.ProduceAsync(GetCartDto(foundCart), default, accessToken!);
        return await Task.FromResult(GetCartDto(foundCart));
    }

    public async Task MarkCartAsBought(Guid cartId)
    {
        var cart = await dataContext.ShoppingCarts.FindAsync(cartId);
        if (cart == null) throw new Exception("Cart is not exist");
        cart.IsBought = true;
        await dataContext.SaveChangesAsync();
    }

    private async Task ChangeAmountToPay(Guid cartId)
    {
        var cart = await dataContext.ShoppingCarts.FirstAsync(x => x.Id == cartId);
        var allOrders = await cache.GetRecordAsync<List<Order>>(cartId.ToString());
        var orders = allOrders?.Where(x => x.CartId == cartId).ToList();
        var isCartHaveOrders = orders?.Any(x => x.CartId == cartId) ?? false;
        if (isCartHaveOrders)
        {
            var sum = 0;
            foreach (var order in orders!)
            {
                var foundProduct = await productCatalog.Get(order.OrderedProductId);
                sum += order.Quantity * foundProduct?.Cost ?? 0;
            }

            cart.AmountToPay = sum;
        }
        else
        {
            cart.AmountToPay = 0;
        }
        await dataContext.SaveChangesAsync();
    }

    private PlaceDto GetPlaceDto(Place place) => new()
    {
        Id = place.Id,
        Address = place.Address,
        WorkingTime = place.WorkingTime
    };

    private CartDto GetCartDto(Cart cart) => new()
    {
        Id = cart.Id,
        UserId = cart.UserId,
        PlaceId = cart.PlaceId,
        AmountToPay = cart.AmountToPay,
        IsConfirmed = cart.IsConfirmed,
        IsBought = cart.IsBought
    };
    
    private OrderDto GetOrderDto(Order order) => new()
    {
        Id = order.Id,
        CartId = order.CartId,
        OrderedProductId = order.OrderedProductId,
        Quantity = order.Quantity
    };
}