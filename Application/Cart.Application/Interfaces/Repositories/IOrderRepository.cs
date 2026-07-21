using Cart.Application.Dtos;
using Model;

namespace Cart.Application.Interfaces.Repositories;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetCartOrdersAsync(Guid cartId);
    Task<Order?> Get(GetOrderDto orderDto);
    Task<Order> AddOrderAsync(CreateOrderDto orderDto);
    Task<Order> CreateOrderAsync(CreateOrderDto createOrderDto);
    Task DeleteOrderAsync(DeleteOrderDto deleteOrderDto);
    Task UpdateCartOrdersAsync(Guid cartId, IEnumerable<Order> orders);
}