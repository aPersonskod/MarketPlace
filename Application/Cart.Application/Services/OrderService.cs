using Cart.Application.Dtos;
using Cart.Application.Interfaces;
using Cart.Application.Interfaces.Services;
using Cart.Application.Mappings;

namespace Cart.Application.Services;

public class OrderService(IUnitOfWork unitOfWork) : IOrderService
{
    public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync(Guid cartId)
    {
        var orders = await unitOfWork.OrderRepository.GetAllOrdersAsync(cartId);
        return orders.Select(x => x.ToDto());
    }

    public async Task<OrderDto> AddOrderAsync(CreateOrderDto orderDto)
    {
        // todo productService getProduct
        var productCost = 5;
        var order = Model.Order.CreateOrder(
            orderDto.CartId,
            orderDto.OrderedProductId,
            orderDto.Quantity);
        var createdOrder = await unitOfWork.OrderRepository.AddOrderAsync(order);
        await unitOfWork.CartRepository.UpdateAmountToPayAsync(createdOrder.CartId, productCost);
        await unitOfWork.CompleteAsync();
        return createdOrder.ToDto();
    }

    public async Task DeleteOrderAsync(Guid cartId, Guid orderId)
    {
        await unitOfWork.OrderRepository.DeleteOrderAsync(cartId, orderId);
        await unitOfWork.CompleteAsync();
    }
}