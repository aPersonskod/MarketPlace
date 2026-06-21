using Microsoft.Extensions.DependencyInjection;
using Cart.Application.Interfaces.Services;
using Cart.Application.Services;

namespace Cart.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddCartApplication(this IServiceCollection services)
    {
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IPlaceService, PlaceService>();
        services.AddScoped<IProductService, ProductService>();
        return services;
    }
}