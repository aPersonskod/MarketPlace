using Microsoft.Extensions.DependencyInjection;
using Cart.Application.Interfaces.Services;
using Cart.Application.Services;
using Cart.Application.Validation;
using FluentValidation;

namespace Cart.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddCartApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateOrderDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<DeleteOrderDtoValidator>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IPlaceService, PlaceService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IBuyReportService, BuyReportService>();
        return services;
    }
}