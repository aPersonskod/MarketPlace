using Cart.Application.Dtos;
using Cart.Application.Interfaces;
using Cart.Application.Interfaces.Repositories;
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
            var order = await unitOfWork.OrderRepository.Get(new GetOrderDto()
            {
                CartId = orderDto.CartId,
                OrderedProductId = orderDto.OrderedProductId
            });
            var newOrder = order == null
                ? await unitOfWork.OrderRepository.CreateOrderAsync(orderDto)
                : await unitOfWork.OrderRepository.AddOrderAsync(orderDto);
            await unitOfWork.CompleteAsync();
            var costCollection = await GetCostCollection(newOrder.CartId);
            await unitOfWork.CartRepository.UpdateAmountToPayAsync(orderDto.CartId, costCollection);
            await unitOfWork.CommitTransactionAsync();
            return newOrder.ToDto();
        }
        catch (Exception)
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
        catch (Exception)
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public Task UpdateOrdersAsync(Guid cartId) => Task.CompletedTask;

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

public class CachedOrderService(
    CartRepositoryResolver cartAccessor,
    OrderRepositoryResolver orderAccessor,
    IProductService productService
) : IOrderService
{
    private readonly ICartRepository _cachedCartRepository = cartAccessor(CartRepositoryKeys.CachedCart)!;
    private readonly IOrderRepository _orderRepository = orderAccessor(OrderRepositoryKeys.Order)!;
    private readonly IOrderRepository _cachedOrderRepository = orderAccessor(OrderRepositoryKeys.CachedOrder)!;
    public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync(Guid cartId)
    {
        var cartOrders = await _cachedOrderRepository.GetCartOrdersAsync(cartId);
        return cartOrders.Select(x => x.ToDto());
    }

    public async Task<OrderDto> AddOrderAsync(CreateOrderDto orderDto)
    {
        // check if product exist
        await productService.GetProductByIdAsync(orderDto.OrderedProductId);
        // add or create order
        var order = await _cachedOrderRepository.Get(new GetOrderDto()
        {
            CartId = orderDto.CartId,
            OrderedProductId = orderDto.OrderedProductId
        });
        // add/create order
        var newOrder = order == null
            ? await _cachedOrderRepository.CreateOrderAsync(orderDto)
            : await _cachedOrderRepository.AddOrderAsync(orderDto);
        var costCollection = await GetCostCollection(newOrder.CartId);
        // update amount to pay in cart
        await _cachedCartRepository.UpdateAmountToPayAsync(orderDto.CartId, costCollection);
        return newOrder.ToDto();
    }

    public async Task DeleteOrderAsync(DeleteOrderDto deleteOrderDto)
    {
        await _cachedOrderRepository.DeleteOrderAsync(deleteOrderDto);
        var costCollection = await GetCostCollection(deleteOrderDto.CartId);
        await _cachedCartRepository.UpdateAmountToPayAsync(deleteOrderDto.CartId, costCollection);
    }

    public async Task UpdateOrdersAsync(Guid cartId)
    {
        var cachedOrders = await _cachedOrderRepository.GetCartOrdersAsync(cartId);
        var orders = cachedOrders.ToList();
        // update db orders
        await _orderRepository.UpdateCartOrdersAsync(cartId, orders);
        // update cache orders
        await _cachedOrderRepository.UpdateCartOrdersAsync(cartId, orders);
    }
    
    private async Task<IEnumerable<(int productCost, int productQuantity)>?> GetCostCollection(Guid cartId)
    {
        var cartOrders = await _cachedOrderRepository.GetCartOrdersAsync(cartId);
        var costCollection = new List<(int productCost, int productQuantity)>();
        foreach (var cartOrder in cartOrders)
        {
            var productDto = await productService.GetProductByIdAsync(cartOrder.OrderedProductId);
            costCollection.Add((productDto.Cost, cartOrder.Quantity));
        }

        return costCollection;
    }
}