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
        }
        else
        {
            var product = await productCatalog.Get(productId);
            if (product != null)
            {
                var existingOrder = dataContext.Orders.FirstOrDefault(o => o.OrderedProduct?.Id == productId);
                if (existingOrder != null)
                {
                    existingOrder.Quantity += quantity;
                }
                else
                {
                    order = new Order()
                    {
                        Id = Guid.NewGuid(),
                        OrderedProduct = product,
                        Quantity = quantity
                    };
                    dataContext.Orders.Add(order);
                }
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
        }

        return Get();
    }
}