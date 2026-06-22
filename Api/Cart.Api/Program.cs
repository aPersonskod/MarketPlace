using System.Security.Claims;
using Cart.Api.Middleware.Error;
using Cart.Application;
using Cart.Application.Dtos;
using Cart.Application.Interfaces.Services;
using Cart.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCartInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddCartApplication();
builder.Services.AddCors(o =>
    o.AddPolicy("CorsPolicy", b =>
    {
        b.AllowAnyMethod()
            .SetIsOriginAllowed(_ => true)
            .AllowAnyHeader()
            .AllowCredentials();
    }));
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler();

#region Cart

app.MapGet("/api/cart-service/get-bought-carts", async (ClaimsPrincipal user, ICartService cartService) =>
    {
        var idStr = user.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
        if (!Guid.TryParse(idStr, out var userId)) return Results.Unauthorized();
        return Results.Ok(await cartService.GetBoughtCartsAsync(userId));
    })
    .WithDescription("Get bought carts")
    .WithName("GetBoughtCarts")
    .RequireAuthorization()
    .WithOpenApi();

app.MapGet("/api/cart-service/get-cart", async (ClaimsPrincipal user, ICartService cartService) =>
    {
        var idStr = user.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
        if (!Guid.TryParse(idStr, out var userId)) return Results.Unauthorized();
        return Results.Ok(await cartService.GetCartByUserIdAsync(userId));
    })
    .WithDescription("Get cart")
    .WithName("GetCart")
    .RequireAuthorization()
    .WithOpenApi();

#endregion

#region Order

app.MapGet("/api/cart-service/get-cart-orders/{cartId:guid}", async (IOrderService orderService, Guid cartId)
        => Results.Ok(await orderService.GetAllOrdersAsync(cartId)))
    .WithDescription("Get cart orders")
    .WithName("GetCartOrders")
    .WithOpenApi();

app.MapPost("/api/cart-service/add-order", async (IOrderService orderService, [FromBody] CreateOrderDto createOrderDto)
        => Results.Ok(await orderService.AddOrderAsync(createOrderDto)))
    .WithDescription("Add order")
    .WithName("AddOrder")
    .WithOpenApi();

app.MapDelete("/api/cart-service/delete-order",
        async (IOrderService orderService, [FromBody] DeleteOrderDto deleteOrderDto) =>
        {
            await orderService.DeleteOrderAsync(deleteOrderDto);
            return Results.Ok();
        })
    .WithDescription("Delete order")
    .WithName("DeleteOrder")
    .WithOpenApi();

#endregion


app.Run();