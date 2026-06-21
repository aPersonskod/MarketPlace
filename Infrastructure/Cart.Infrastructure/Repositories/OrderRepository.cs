using Cart.Application.Interfaces.Repositories;
using Cart.Infrastructure.Data;
using Model;

namespace Cart.Infrastructure.Repositories;

public class OrderRepository(AppDbContext context) : IOrderRepository
{
    public Task<IEnumerable<Order>> GetAllOrdersAsync(Guid cartId)
    {
        throw new NotImplementedException();
    }

    public Task<Order> AddOrderAsync(Order order)
    {
        throw new NotImplementedException();
    }

    public Task DeleteOrderAsync(Guid cartId, Guid orderId)
    {
        throw new NotImplementedException();
    }
}