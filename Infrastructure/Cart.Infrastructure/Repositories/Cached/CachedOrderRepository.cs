using Cart.Application.Dtos;
using Cart.Application.Interfaces;
using Cart.Application.Interfaces.Repositories.Cached;
using Microsoft.Extensions.Caching.Distributed;
using Model;
using Model.Extensions;
using Model.SharedExceptions;

namespace Cart.Infrastructure.Repositories.Cached;

public class CachedOrderRepository(IUnitOfWork unitOfWork, IDistributedCache cache) : ICachedOrderRepository
{
    private const string NsCacheKey = "NSCartIds"; // not synced cartIds key
    private static string CacheKey (Guid cartId) => $"orders:{cartId}";
    private async Task AddNsCartIdAsync(Guid cartId)
    {
        var nsCartIds = await cache.GetRecordAsync<IEnumerable<Guid>>(NsCacheKey);
        if (nsCartIds == null)
        {
            await cache.SetRecordAsync<IEnumerable<Guid>>(NsCacheKey, [cartId]);
        }
        else
        {
            var cartIds = nsCartIds.ToList();
            cartIds.Add(cartId);
            await cache.SetRecordAsync(NsCacheKey, cartIds.Distinct());
        }
    }

    public async Task<IEnumerable<Order>> GetCartOrdersAsync(Guid cartId)
    {
        // get cache data
        var cachedCartOrders = await cache.GetRecordAsync<IEnumerable<Order>>(CacheKey(cartId));
        if (cachedCartOrders == null)
        {
            // get db data
            var cartOrders = await unitOfWork.OrderRepository.GetCartOrdersAsync(cartId);
            var cartOrdersList = cartOrders.ToList();
            if (cartOrdersList.Count != 0)
            {
                // set db data to cache
                await cache.SetRecordAsync(CacheKey(cartId), cartOrdersList);
                cachedCartOrders = await cache.GetRecordAsync<IEnumerable<Order>>(CacheKey(cartId));
            }
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
        await cache.SetRecordAsync(CacheKey(orderDto.CartId), cartOrders);
        await AddNsCartIdAsync(orderDto.CartId);
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
        await cache.SetRecordAsync(CacheKey(createOrderDto.CartId), cartOrders);
        await AddNsCartIdAsync(createOrderDto.CartId);
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
        await cache.SetRecordAsync(CacheKey(deleteOrderDto.CartId), cartOrders);
        await AddNsCartIdAsync(deleteOrderDto.CartId);
    }
}