using Cart.Application.Dtos;
using Cart.Application.Interfaces;
using Cart.Application.Interfaces.Services;
using Cart.Application.Mappings;

namespace Cart.Application.Services;

public class OrderService(IUnitOfWork unitOfWork, IProductService productService) : IOrderService
{
    public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync(Guid cartId)
    {
        var orders = await unitOfWork.OrderRepository.GetAllOrdersAsync(cartId);
        return orders.Select(x => x.ToDto());
    }

    public async Task<OrderDto> AddOrderAsync(CreateOrderDto orderDto)
    {
        // check if product exist
        await productService.GetProductByIdAsync(orderDto.OrderedProductId);
        // add or create order
        var order = await unitOfWork.OrderRepository.AddOrderAsync(orderDto) 
                    ?? await unitOfWork.OrderRepository.CreateOrderAsync(orderDto);
        await unitOfWork.CompleteAsync();
        var cartOrders = await unitOfWork.OrderRepository.GetAllOrdersAsync(order.CartId);
        var costCollection = new List<(int productCost, int productQuantity)>();
        foreach (var cartOrder in cartOrders)
        {
            var productDto = await productService.GetProductByIdAsync(cartOrder.OrderedProductId);
            costCollection.Add((productDto.Cost, cartOrder.Quantity));
        }
        await unitOfWork.CartRepository.UpdateAmountToPayAsync(order.CartId, costCollection);
        await unitOfWork.CompleteAsync();
        return order.ToDto();
    }
    public async Task DeleteOrderAsync(DeleteOrderDto deleteOrderDto)
    {
        await unitOfWork.OrderRepository.DeleteOrderAsync(deleteOrderDto);
        await unitOfWork.CompleteAsync();
    }
}