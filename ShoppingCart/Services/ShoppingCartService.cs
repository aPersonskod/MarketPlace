using Microsoft.EntityFrameworkCore;
using Models;
using Models.Dtos;
using Models.Interfaces;

namespace ShoppingCart.Services;

public class ShoppingCartService(
    DataContext dataContext,
    IProductCatalog productCatalog,
    UserClientService userService,
    IKafkaProducer<CartDto> kafkaCartProducer) : IShoppingCart
{
    public Task<IEnumerable<CartDto>> GetCarts() => Task.FromResult(dataContext.ShoppingCarts.Select(GetCartDto));
    public Task<IEnumerable<PlaceDto>> GetPlaces() => Task.FromResult(dataContext.Places.Select(GetPlaceDto));
    public async Task<PlaceDto> GetPlace(Guid placeId)
    {
        var place = await dataContext.Places.FindAsync(placeId);
        if (place == null) throw new Exception("Place not found");
        return await Task.FromResult(GetPlaceDto(place));
    }

    public Task<IEnumerable<OrderDto>> GetOrders(Guid cartId) =>
        Task.FromResult(dataContext.Orders.Where(x => x.CartId == cartId).AsEnumerable().Select(GetOrderDto));

    public async Task<CartDto> GetCart(Guid userId)
    {
        var foundUserDto = await userService.GetUser(userId);
        if (foundUserDto == null) throw new Exception("User not found");
        var cart = await dataContext.ShoppingCarts.FirstOrDefaultAsync(c => c.UserId == userId && !c.IsBought && !c.IsConfirmed);
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

    public async Task<CartDto> AddOrder(Guid userId, Guid productId, int quantity)
    {
        var cart = await GetCart(userId);
        var foundOrder = await dataContext.Orders.FirstOrDefaultAsync(x => x.CartId == cart.Id && x.OrderedProductId == productId);
        if (foundOrder != null)
        {
            var order = await dataContext.Orders.FindAsync(foundOrder.Id);
            order!.Quantity += quantity;
        }
        else
        {
            var product = await productCatalog.Get(productId);
            var newOrder = new Order()
            {
                Id = Guid.NewGuid(),
                CartId = cart.Id,
                OrderedProductId = product.Id,
                Quantity = quantity
            };
            await dataContext.Orders.AddAsync(newOrder);
        }
        await dataContext.SaveChangesAsync();

        await ChangeAmountToPay(cart.Id);
        return await GetCart(userId);
    }
    
    public async Task<CartDto> DeleteOrder(Guid userId, Guid productId)
    {
        var foundCart = await GetCart(userId);
        var cart = await dataContext.ShoppingCarts.FindAsync(foundCart.Id);
        var order = await dataContext.Orders.FirstOrDefaultAsync(x => x.OrderedProductId == productId);
        if (order == null) throw new Exception("Order not found");
        dataContext.Orders.Remove(order);
        await dataContext.SaveChangesAsync();
        await ChangeAmountToPay(cart!.Id);
        return await Task.FromResult(GetCartDto(cart));
    }

    [Obsolete("synchronous task that is too slow")]
    public async Task<CartDto> ConfirmCart(Guid userId, Guid placeId)
    {
        var cart = await GetCart(userId);
        
        var isCartNotEmpty = await dataContext.Orders.AnyAsync(x => x.CartId == cart.Id);
        if (!isCartNotEmpty) throw new Exception($"Cart has no orders !!!");
        
        var foundUser = await userService.GetUser(userId);
        var isUserHasEnoughMoney = foundUser!.Wallet >= cart.AmountToPay;
        if (!isUserHasEnoughMoney) throw new Exception($"You have not enough money !!!");
        
        var foundCart = await dataContext.ShoppingCarts.FindAsync(cart.Id);
        foundCart!.IsConfirmed = true;
        await dataContext.SaveChangesAsync();
        return await Task.FromResult(GetCartDto(foundCart));
    }

    public async Task<CartDto> ConfirmAndBuyCart(Guid userId, Guid? placeId)
    {
        var cart = await GetCart(userId);
        
        var isCartNotEmpty = await dataContext.Orders.AnyAsync(x => x.CartId == cart.Id);
        if (!isCartNotEmpty) throw new Exception($"Cart has no orders !!!");
        
        var foundUser = await userService.GetUser(userId);
        var isUserHasEnoughMoney = foundUser!.Wallet >= cart.AmountToPay;
        if (!isUserHasEnoughMoney) throw new Exception($"You have not enough money !!!");
        
        var foundCart = await dataContext.ShoppingCarts.FindAsync(cart.Id);
        if(foundCart == null) throw new Exception($"Undefined cart !!!");
        foundCart.PlaceId = placeId;
        if (foundCart.PlaceId == null) throw new Exception($"Cart's place is empty !!!");
        foundCart.IsConfirmed = true;
        await dataContext.SaveChangesAsync();
        await kafkaCartProducer.ProduceAsync(GetCartDto(foundCart), default);
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
        var orders = dataContext.Orders.Where(x => x.CartId == cartId);
        var isCartHaveOrders = await orders.AnyAsync(x => x.CartId == cartId);
        if (isCartHaveOrders)
        {
            var sum = 0;
            foreach (var order in orders)
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