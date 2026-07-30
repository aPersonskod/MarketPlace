using Cart.Application.Dtos;
using Model;

namespace Cart.Application.Interfaces.Repositories.Cached;

public interface ICachedOrderRepository
{
    Task<IEnumerable<Order>> GetCartOrdersAsync(Guid cartId);
    Task<Order?> Get(GetOrderDto orderDto);
    Task<Order> AddOrderAsync(CreateOrderDto orderDto);
    Task<Order> CreateOrderAsync(CreateOrderDto createOrderDto);
    Task DeleteOrderAsync(DeleteOrderDto deleteOrderDto);
}