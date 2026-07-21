using Cart.Application.Interfaces.Repositories;
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
        services.AddScoped<ICartService, CachedCartService>();
        services.AddScoped<IOrderService, CachedOrderService>();
        services.AddScoped<IPlaceService, PlaceService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IBuyReportService, BuyReportService>();
        return services;
    }
}

public delegate ICartRepository? CartRepositoryResolver(CartRepositoryKeys key);
public delegate IOrderRepository? OrderRepositoryResolver(OrderRepositoryKeys key);

public enum CartRepositoryKeys
{
    Cart,
    CachedCart
}

public enum OrderRepositoryKeys
{
    Order,
    CachedOrder
}