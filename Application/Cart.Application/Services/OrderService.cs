using Cart.Application.Dtos;
using Cart.Application.Interfaces;
using Cart.Application.Interfaces.Services;
using Cart.Application.Mappings;

namespace Cart.Application.Services;

public class OrderService(IUnitOfWork unitOfWork, IProductService productService) : IOrderService
{
    public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync(Guid cartId)
    {
        var orders = await unitOfWork.OrderRepository.GetCartOrdersAsync(cartId);
        return orders.Select(x => x.ToDto());
    }

    public async Task<OrderDto> AddOrderAsync(CreateOrderDto orderDto)
    {
        // check if product exist
        await productService.GetProductByIdAsync(orderDto.OrderedProductId);
        // add or create order
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var order = await unitOfWork.OrderRepository.Get(orderDto);
            var newOrder = order == null
                ? await unitOfWork.OrderRepository.CreateOrderAsync(orderDto)
                : await unitOfWork.OrderRepository.AddOrderAsync(orderDto);
            await unitOfWork.CompleteAsync();
            var costCollection = await GetCostCollection(newOrder.CartId);
            await unitOfWork.CartRepository.UpdateAmountToPayAsync(orderDto.CartId, costCollection);
            await unitOfWork.CommitTransactionAsync();
            return newOrder.ToDto();
        }
        catch (Exception e)
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
    public async Task DeleteOrderAsync(DeleteOrderDto deleteOrderDto)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            await unitOfWork.OrderRepository.DeleteOrderAsync(deleteOrderDto);
            await unitOfWork.CompleteAsync();
            var costCollection = await GetCostCollection(deleteOrderDto.CartId);
            await unitOfWork.CartRepository.UpdateAmountToPayAsync(deleteOrderDto.CartId, costCollection);
            await unitOfWork.CommitTransactionAsync();
        }
        catch (Exception e)
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    private async Task<IEnumerable<(int productCost, int productQuantity)>?> GetCostCollection(Guid cartId)
    {
        var cartOrders = await unitOfWork.OrderRepository.GetCartOrdersAsync(cartId);
        var costCollection = new List<(int productCost, int productQuantity)>();
        foreach (var cartOrder in cartOrders)
        {
            var productDto = await productService.GetProductByIdAsync(cartOrder.OrderedProductId);
            costCollection.Add((productDto.Cost, cartOrder.Quantity));
        }
        return costCollection;
    }
}