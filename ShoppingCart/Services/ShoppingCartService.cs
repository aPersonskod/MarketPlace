using Microsoft.EntityFrameworkCore;
using Models;
using Models.Dtos;
using Models.Interfaces;

namespace ShoppingCart.Services;

public class ShoppingCartService(
    DataContext dataContext,
    IProductCatalog productCatalog,
    UserClientService userService,
    IKafkaProducer<Cart> kafkaCartProducer) : IShoppingCart
{
    public Task<IEnumerable<Cart>> Get() => Task.FromResult<IEnumerable<Cart>>(dataContext.ShoppingCarts);
    public Task<IEnumerable<Place>> GetPlaces() => Task.FromResult<IEnumerable<Place>>(dataContext.Places);

    public async Task<Cart> Get(Guid userId)
    {
        var foundUser = await userService.GetUser(userId);
        if (foundUser == null) throw new Exception("User not found");
        var cart = await dataContext.ShoppingCarts.FirstOrDefaultAsync(c => c.UserId == userId && !c.IsBought && !c.IsConfirmed);
        if (cart != null) return await Task.FromResult(cart);
        var newCart = new Cart() { Id = Guid.NewGuid(), UserId = foundUser.Id };
        await dataContext.ShoppingCarts.AddAsync(newCart);
        await dataContext.SaveChangesAsync();
        return await Task.FromResult(newCart);
    }

    public async Task<Cart> AddOrder(Guid userId, Guid productId, int quantity)
    {
        var cart = await Get(userId);
        var foundOrderId = await dataContext.Orders.FirstOrDefaultAsync(x => x.CartId == cart.Id);
        if (foundOrderId != null)
        {
            var order = await dataContext.Orders.FindAsync(foundOrderId);
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

        await ChangeAmountToPay(cart.Id);
        return await Get(userId);
    }

    [Obsolete("synchronous task that is too slow")]
    public async Task<Cart> ConfirmCart(Guid userId, Guid placeId)
    {
        var cart = await Get(userId);
        
        var isCartNotEmpty = await dataContext.Orders.AnyAsync(x => x.CartId == cart.Id);
        if (!isCartNotEmpty) throw new Exception($"Cart has no orders !!!");
        
        var foundUser = await userService.GetUser(userId);
        var isUserHasEnoughMoney = foundUser!.Wallet >= cart.AmountToPay;
        if (!isUserHasEnoughMoney) throw new Exception($"You have not enough money !!!");
        
        var foundCart = await dataContext.ShoppingCarts.FindAsync(cart.Id);
        foundCart!.IsConfirmed = true;
        await dataContext.SaveChangesAsync();
        return await Task.FromResult(cart);
    }

    public async Task<Cart> ConfirmAndBuyCart(Guid userId, Guid placeId)
    {
        var cart = await Get(userId);
        
        var isCartNotEmpty = await dataContext.Orders.AnyAsync(x => x.CartId == cart.Id);
        if (!isCartNotEmpty) throw new Exception($"Cart has no orders !!!");
        
        var foundUser = await userService.GetUser(userId);
        var isUserHasEnoughMoney = foundUser!.Wallet >= cart.AmountToPay;
        if (!isUserHasEnoughMoney) throw new Exception($"You have not enough money !!!");
        
        var foundCart = await dataContext.ShoppingCarts.FindAsync(cart.Id);
        foundCart!.IsConfirmed = true;
        await dataContext.SaveChangesAsync();
        await kafkaCartProducer.ProduceAsync(foundCart, default);
        return await Task.FromResult(cart);
    }

    public async Task MarkCartAsBought(Guid cartId)
    {
        var cart = await dataContext.ShoppingCarts.FindAsync(cartId);
        if (cart == null) throw new Exception("Cart is not exist");
        cart.IsBought = true;
        await dataContext.SaveChangesAsync();
    }

    public async Task<Cart> DeleteOrder(Guid userId, Guid productId)
    {
        var foundCart = await Get(userId);
        var cart = await dataContext.ShoppingCarts.FindAsync(foundCart.Id);
        var order = await dataContext.Orders.FirstOrDefaultAsync(x => x.OrderedProductId == productId);
        if (order == null) throw new Exception("Order not found");
        dataContext.Orders.Remove(order);
        await dataContext.SaveChangesAsync();
        await ChangeAmountToPay(cart!.Id);
        return await Task.FromResult(cart);
    }

    private async Task ChangeAmountToPay(Guid cartId)
    {
        var cart = dataContext.ShoppingCarts.First(x => x.Id == cartId);
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
}