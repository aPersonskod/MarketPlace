using Model;

namespace Cart.Application.Interfaces.Repositories;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllOrdersAsync(Guid cartId);
    Task<Order> AddOrderAsync(Order order);
    Task DeleteOrderAsync(Guid cartId, Guid orderId);
}