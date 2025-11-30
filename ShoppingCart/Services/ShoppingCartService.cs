using Models;
using Models.Interfaces;

namespace ShoppingCart.Services;

public class ShoppingCartService(DataContext dataContext, IProductCatalog productCatalog) : IShoppingCart
{
    public Task<IEnumerable<Cart>> Get() => Task.FromResult<IEnumerable<Cart>>(dataContext.ShoppingCarts);
    public Task<IEnumerable<Place>> GetPlaces() => Task.FromResult<IEnumerable<Place>>(dataContext.Places);

    public async Task<Cart> Get(Guid userId)
    {
        var carts = dataContext.ShoppingCarts;
        var cart = carts.FirstOrDefault(c => c.User.Id == userId && !c.IsConfirmed);
        var userService = new UserClientService();
        if (cart != null)
        {
            cart.User = await userService.GetUser(userId) ?? cart.User;
            return await Task.FromResult(cart);
        }
        var foundUser = await userService.GetUser(userId);
        if (foundUser == null) throw new NullReferenceException("User not found");
        var newCart = new Cart() { Id = Guid.NewGuid(), User = foundUser };
        dataContext.ShoppingCarts.Add(newCart);
        return await Task.FromResult(newCart);
    }

    public async Task<Cart> AddOrder(Guid userId, Guid productId, int quantity)
    {
        var cart = await Get(userId);
        var order = cart.Orders.FirstOrDefault(o => o.OrderedProduct?.Id == productId);
        if (order != null)
        {
            order.Quantity += quantity;
        }
        else
        {
            var product = await productCatalog.Get(productId);
            if (product != null)
            {
                order = new Order()
                {
                    Id = Guid.NewGuid(),
                    OrderedProduct = product,
                    Quantity = quantity
                };
                dataContext.ShoppingCarts.First(x => x.Id == cart.Id).Orders.Add(order);
                //cart.Orders.Add(order);
                //dataContext.ShoppingCarts.Add(cart);
            }
        }

        return await Get(userId);
    }

    public Task<Cart> ConfirmCart(Guid userId, Guid placeId)
    {
        var cart = dataContext.ShoppingCarts.FirstOrDefault(c => c.User.Id == userId && !c.IsConfirmed);
        cart!.Place = dataContext.Places.FirstOrDefault(p => p.Id == placeId)!;
        cart.IsConfirmed = true;
        return Task.FromResult(cart);
    }

    public async Task<Cart> DeleteOrder(Guid userId, Guid productId)
    {
        var cartId = (await Get(userId)).Id;
        var cart = dataContext.ShoppingCarts.First(x => x.Id == cartId);
        var order = cart.Orders.FirstOrDefault(o => o.OrderedProduct?.Id == productId);
        if (order == null) throw new Exception("Order not found");
        cart.Orders.Remove(order);
        return await Task.FromResult(cart);
    }
}

public class UserClientService
{
    private const string BaseAddress = "https://localhost:7004/UserManipulations";
    public async Task<User?> GetUser(Guid userId)
    {
        var query = $"{BaseAddress}/{userId}";
        try
        {
            var result = await query.GetQuery<User>();
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}