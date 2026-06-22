using Cart.Application.Dtos;
using Cart.Application.Interfaces.Repositories;
using Cart.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Model.SharedExceptions;
using Model;

namespace Cart.Infrastructure.Repositories;

public class OrderRepository(AppDbContext context) : IOrderRepository
{
    public async Task<IEnumerable<Order>> GetAllOrdersAsync(Guid cartId) 
        => await context.Orders.Where(x => x.CartId == cartId).ToListAsync();
    public async Task<Order?> Get(CreateOrderDto orderDto) 
        => await context.Orders.FirstOrDefaultAsync(x => 
            x.CartId == orderDto.CartId && x.OrderedProductId == orderDto.OrderedProductId);

    public async Task<Order?> AddOrderAsync(CreateOrderDto orderDto)
    {
        var foundOrder = await Get(orderDto);
        if (foundOrder == null) return foundOrder;
        foundOrder.Quantity += orderDto.Quantity;
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
        if (order == null) throw new NotFoundException("Order not found");
        context.Orders.Remove(order);
    }
}