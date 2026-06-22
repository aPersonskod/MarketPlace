using Cart.Application.Dtos;
using Model;

namespace Cart.Application.Interfaces.Repositories;

public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllOrdersAsync(Guid cartId);
    Task<Order?> Get(CreateOrderDto orderDto);
    Task<Order?> AddOrderAsync(CreateOrderDto orderDto);
    Task<Order> CreateOrderAsync(CreateOrderDto createOrderDto);
    Task DeleteOrderAsync(DeleteOrderDto deleteOrderDto);
}