using Cart.Application.Dtos;
using Cart.Application.Interfaces;
using Cart.Application.Interfaces.Services;
using Cart.Application.Mappings;

namespace Cart.Application.Services;

public class OrderService(IUnitOfWork unitOfWork, ProductService productService) : IOrderService
{
    public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync(Guid cartId)
    {
        var orders = await unitOfWork.OrderRepository.GetAllOrdersAsync(cartId);
        return orders.Select(x => x.ToDto());
    }

    public async Task<OrderDto> AddOrderAsync(CreateOrderDto orderDto)
    {
        var createdOrder = await unitOfWork.OrderRepository.AddOrderAsync(orderDto);
        var cartOrders = await unitOfWork.OrderRepository.GetAllOrdersAsync(createdOrder.CartId);
        var costCollection = new List<(int productCost, int productQuantity)>();
        foreach (var cartOrder in cartOrders)
        {
            var productDto = await productService.GetProductByIdAsync(cartOrder.OrderedProductId);
            costCollection.Add((productDto.Cost, orderDto.Quantity));
        }
        await unitOfWork.CartRepository.UpdateAmountToPayAsync(createdOrder.CartId, costCollection);
        await unitOfWork.CompleteAsync();
        return createdOrder.ToDto();
    }
    public async Task DeleteOrderAsync(DeleteOrderDto deleteOrderDto)
    {
        await unitOfWork.OrderRepository.DeleteOrderAsync(deleteOrderDto);
        await unitOfWork.CompleteAsync();
    }
}