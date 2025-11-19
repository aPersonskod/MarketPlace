using Models;

namespace ShoppingCart.Services;

public class ShoppingCartService(DataContext dataContext, IProductCatalog productCatalog) : IShoppingCart
{
    public async Task<Cart> Get() => await Task.FromResult(new Cart() { Orders = dataContext.Orders });

    public async Task<Cart> AddOrder(Guid productId, int quantity)
    {
        var order = dataContext.Orders.FirstOrDefault(o => o.Id == productId);
        if (order != null)
        {
            order.Quantity = quantity;
            CreateEvent(CartEventType.OrderChanged, order);
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
                dataContext.Orders.Add(order);
                CreateEvent(CartEventType.OrderAdded, order);
            }
        }

        return await Get();
    }

    public Task<Cart> DeleteOrder(Guid productId)
    {
        var order = dataContext.Orders.FirstOrDefault(o => o.Id == productId);
        if (order != null)
        {
            dataContext.Orders.Remove(order);
            CreateEvent(CartEventType.OrderRemoved, order);
        }

        return Get();
    }

    public IEnumerable<CartEvent> GetCartEvents(long timestamp)
        => dataContext.CartEvents.Where(e => e.TimeStamp > timestamp);

    private void CreateEvent(CartEventType cartEventType, Order order)
    {
        dataContext.CartEvents.Add(new CartEvent()
        {
            Order = order,
            Type = cartEventType,
            TimeStamp = DateTime.Now.Ticks,
            Time = DateTime.Now
        });
    }
}