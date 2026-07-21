using Cart.Application.Dtos;
using Cart.Application.Interfaces;
using Cart.Application.Interfaces.Repositories;
using Cart.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Model.SharedExceptions;
using Model.Extensions;
using Model;

namespace Cart.Infrastructure.Repositories;

public class OrderRepository(AppDbContext context) : IOrderRepository
{
    public async Task<IEnumerable<Order>> GetCartOrdersAsync(Guid cartId) 
        => await context.Orders.Where(x => x.CartId == cartId).ToListAsync();
    public async Task<Order?> Get(GetOrderDto orderDto) 
        => await context.Orders.FirstOrDefaultAsync(x => 
            x.CartId == orderDto.CartId && x.OrderedProductId == orderDto.OrderedProductId);

    public async Task<Order> AddOrderAsync(CreateOrderDto orderDto)
    {
        var foundOrder = await Get(new GetOrderDto()
        {
            CartId = orderDto.CartId,
            OrderedProductId = orderDto.OrderedProductId
        });
        if (foundOrder == null) throw new NotFoundException("Can't add not found order");
        foundOrder.Quantity = orderDto.Quantity;
        return foundOrder;
    }

    public async Task<Order> CreateOrderAsync(CreateOrderDto createOrderDto)
    {
        var order = Order.CreateOrder(
            createOrderDto.CartId,
            createOrderDto.OrderedProductId,
            createOrderDto.Quantity);
        await context.Orders.AddAsync(order);
        return order;
    }
    public async Task DeleteOrderAsync(DeleteOrderDto deleteOrderDto)
    {
        var order = await context.Orders.FirstOrDefaultAsync(x =>
            x.CartId == deleteOrderDto.CartId && x.OrderedProductId == deleteOrderDto.OrderedProductId);
        if (order == null) throw new NotFoundException("Can't delete not found order");
        context.Orders.Remove(order);
    }
    public async Task UpdateCartOrdersAsync(Guid cartId, IEnumerable<Order> orders)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var cartOrders = await GetCartOrdersAsync(cartId);
            foreach (var cartOrder in cartOrders)
            {
                await DeleteOrderAsync(new DeleteOrderDto()
                {
                    CartId = cartId,
                    OrderedProductId = cartOrder.OrderedProductId
                });
            }
            foreach (var order in orders)
            {
                await CreateOrderAsync(new CreateOrderDto()
                {
                    CartId = cartId,
                    OrderedProductId = order.OrderedProductId,
                    Quantity = order.Quantity
                });
            }
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}

public class CachedOrderRepository(IUnitOfWork unitOfWork, IDistributedCache cache) : IOrderRepository
{
    private static string CacheKey (Guid cartId) => $"orders:{cartId}";
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
                await cache.SetRecordAsync(CacheKey(cartId), cartOrdersList, isSynced: true);
                cachedCartOrders = await cache.GetRecordAsync<IEnumerable<Order>>(CacheKey(cartId));
            }
        }
        // get cache data
        if (cachedCartOrders == null) throw new NotFoundException("Cached orders not found");
        return cachedCartOrders.Value;
    }
    public async Task<Order?> Get(GetOrderDto orderDto)
    {
        var cartOrders = await GetCartOrdersAsync(orderDto.CartId);
        return cartOrders.FirstOrDefault(x => x.OrderedProductId == orderDto.OrderedProductId);
    }
    public async Task<Order> AddOrderAsync(CreateOrderDto orderDto)
    {
        var foundOrder = await Get(new GetOrderDto()
        {
            CartId = orderDto.CartId,
            OrderedProductId = orderDto.OrderedProductId
        });
        if (foundOrder == null) throw new NotFoundException("Can't add not found cached order");
        var cartOrdersResult = await GetCartOrdersAsync(orderDto.CartId);
        var cartOrders = cartOrdersResult.ToList();
        var foundCartOrder = cartOrders.First(x => x.Id == foundOrder.Id);
        foundCartOrder.Quantity = orderDto.Quantity;
        await cache.SetRecordAsync(CacheKey(orderDto.CartId), cartOrders, isSynced: false);
        return foundOrder;
    }
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
        return order;
    }
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
        /*if (cartOrders.Count == 0)
        {
            await cache.RemoveAsync(CacheKey(deleteOrderDto.CartId));
            var her = await cache.GetRecordAsync<IEnumerable<Order>>(CacheKey(deleteOrderDto.CartId));
        }*/
        await cache.SetRecordAsync(CacheKey(deleteOrderDto.CartId), cartOrders);
    }
    public async Task UpdateCartOrdersAsync(Guid cartId, IEnumerable<Order> orders) 
        => await cache.SetRecordAsync(CacheKey(cartId), orders, isSynced: true);
}