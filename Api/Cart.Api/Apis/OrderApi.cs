using Cart.Application.Dtos;
using Cart.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cart.Api.Apis;

public static class OrderApi
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/cart-service").WithTags("Order");
        
        api.MapGet("/get-cart-orders/{cartId:guid}", async (IOrderService orderService, Guid cartId)
                => Results.Ok(await orderService.GetAllOrdersAsync(cartId)))
            .WithDescription("Get cart orders")
            .WithName("GetCartOrders")
            .RequireAuthorization()
            .WithOpenApi();

        api.MapPost("/add-order", async (IOrderService orderService, [FromBody] CreateOrderDto createOrderDto)
                => Results.Ok(await orderService.AddOrderAsync(createOrderDto)))
            .WithDescription("Add order")
            .WithName("AddOrder")
            .RequireAuthorization()
            .WithOpenApi();

        api.MapDelete("/delete-order",
                async (IOrderService orderService, [FromBody] DeleteOrderDto deleteOrderDto) =>
                {
                    await orderService.DeleteOrderAsync(deleteOrderDto);
                    return Results.NoContent();
                })
            .WithDescription("Delete order")
            .WithName("DeleteOrder")
            .RequireAuthorization()
            .WithOpenApi();
        
        return app;
    }
}