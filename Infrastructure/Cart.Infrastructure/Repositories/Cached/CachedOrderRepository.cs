using Cart.Application.Dtos;
using Cart.Application.Interfaces;
using Cart.Application.Interfaces.Repositories.Cached;
using Cart.Infrastructure.CacheModels;
using Microsoft.Extensions.Caching.Distributed;
using Model;
using Model.Extensions;
using Model.SharedExceptions;

namespace Cart.Infrastructure.Repositories.Cached;

public class CachedOrderRepository(IUnitOfWork unitOfWork, IDistributedCache cache) : ICachedOrderRepository
{
    public async Task<IEnumerable<Order>> GetCartOrdersAsync(Guid cartId)
    {
        // get cache data
        var cachedCartOrders = await cache.GetCartOrdersByCartId(cartId);
        if (cachedCartOrders == null)
        {
            // get db data
            var cartOrders = await unitOfWork.OrderRepository.GetCartOrdersAsync(cartId);
            // set db data to cache
            await cache.SetCartOrdersByCartId(cartId, cartOrders);
            cachedCartOrders = await cache.GetCartOrdersByCartId(cartId);
        }
        // get cache data
        if (cachedCartOrders == null) throw new NotFoundException("Cache does not save cart orders");
        return cachedCartOrders;
    }
    public async Task<Order?> Get(GetOrderDto orderDto)
    {
        var cartOrders = await GetCartOrdersAsync(orderDto.CartId);
        return cartOrders.FirstOrDefault(x => x.OrderedProductId == orderDto.OrderedProductId);
    }
    // just cache
    public async Task<Order> AddOrderAsync(CreateOrderDto orderDto)
    {
        var foundOrder = await Get(new GetOrderDto()
        {
            CartId = orderDto.CartId,
            OrderedProductId = orderDto.OrderedProductId
        });
        if (foundOrder == null) throw new NotFoundException("Can't find added order in DB");
        var allCartOrders = await GetCartOrdersAsync(orderDto.CartId);
        var cartOrders = allCartOrders.ToList();
        cartOrders.First(x => x.Id == foundOrder.Id).Quantity = orderDto.Quantity;
        await cache.SetCartOrdersByCartId(orderDto.CartId, cartOrders);
        await cache.AddChangedCartId(orderDto.CartId);
        return foundOrder;
    }
    // just cache
    public async Task<Order> CreateOrderAsync(CreateOrderDto createOrderDto)
    {
        var order = Order.CreateOrder(
            createOrderDto.CartId,
            createOrderDto.OrderedProductId,
            createOrderDto.Quantity);
        var cartOrdersResult = await GetCartOrdersAsync(createOrderDto.CartId);
        var cartOrders = cartOrdersResult.ToList();
        cartOrders.Add(order);
        await cache.SetCartOrdersByCartId(createOrderDto.CartId, cartOrders);
        await cache.AddChangedCartId(createOrderDto.CartId);
        return order;
    }
    // just cache
    public async Task DeleteOrderAsync(DeleteOrderDto deleteOrderDto)
    {
        var foundOrder = await Get(new GetOrderDto()
        {
            CartId = deleteOrderDto.CartId,
            OrderedProductId = deleteOrderDto.OrderedProductId
        });
        if (foundOrder == null) throw new NotFoundException("Can't delete not found cached order");
        var cartOrdersResult = await GetCartOrdersAsync(deleteOrderDto.CartId);
        var cartOrders = cartOrdersResult.ToList();
        cartOrders.Remove(cartOrders.Single(x => x.Id == foundOrder.Id));
        await cache.SetCartOrdersByCartId(deleteOrderDto.CartId, cartOrders);
        await cache.AddChangedCartId(deleteOrderDto.CartId);
    }
}