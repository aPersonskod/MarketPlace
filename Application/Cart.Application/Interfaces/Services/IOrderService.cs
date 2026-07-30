using Cart.Application.Dtos;

namespace Cart.Application.Interfaces.Services;

public interface IOrderService
{
    Task<IEnumerable<OrderDto>> GetAllOrdersAsync(Guid cartId);
    Task<OrderDto> AddOrderAsync(CreateOrderDto orderDto);
    Task DeleteOrderAsync(DeleteOrderDto deleteOrderDto);
    Task CachedOrdersToDbAsync(Guid cartId);
}