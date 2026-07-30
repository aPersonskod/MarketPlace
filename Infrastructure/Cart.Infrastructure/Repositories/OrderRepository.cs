using Cart.Application.Dtos;
using Cart.Application.Interfaces.Repositories;
using Cart.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Model.SharedExceptions;
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