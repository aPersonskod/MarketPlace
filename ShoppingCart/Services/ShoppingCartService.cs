using Models;

namespace ShoppingCart.Services;

public class ShoppingCartService(DataContext dataContext, IProductCatalog productCatalog) : IShoppingCart
{
    public Task<IEnumerable<Cart>> Get() => Task.FromResult<IEnumerable<Cart>>(dataContext.ShoppingCarts);
    public Task<IEnumerable<Place>> GetPlaces() => Task.FromResult<IEnumerable<Place>>(dataContext.Places);

    public async Task<Cart> Get(Guid userId)
    {
        var carts = await Get();
        var cart = carts.FirstOrDefault(c => c.User?.Id == userId);
        if (cart != null) return await Task.FromResult(cart);
        var userService = new UserClientService();
        var foundUser = await userService.GetUser(userId);
        return await Task.FromResult(new Cart() { Id = Guid.NewGuid(), User = foundUser!});
    }

    public async Task<Cart> AddOrder(Guid userId, Guid productId, Guid placeId, int quantity)
    {
        var cart = await Get(userId);
        cart.Place = dataContext.Places.FirstOrDefault(p => p.Id == placeId);
        var order = cart.Orders.FirstOrDefault(o => o.OrderedProduct?.Id == productId);
        if (order != null)
        {
            order.Quantity += quantity;
            dataContext.ShoppingCarts.First(x => x.User?.Id == userId).Orders.Add(order);
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
                cart.Orders.Add(order);
                dataContext.ShoppingCarts.Add(cart);
            }
        }

        return await Get(userId);
    }

    public async Task DeleteOrder(Guid userId, Guid productId)
    {
        var cart = await Get(userId);
        var order = cart.Orders.FirstOrDefault(o => o.OrderedProduct?.Id == productId);
        if (order != null)
        {
            dataContext.ShoppingCarts.First(x => x.User.Id == userId).Orders.Remove(order);
        }
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