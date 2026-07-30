using System.Reflection;
using HostedService.Application.Interfaces.Services;
using HostedService.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HostedService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddHostedServiceApplication(this IServiceCollection services)
    {
        services.AddSingleton<ICartService, CartService>();
        services.AddSingleton<IOrderService, OrderService>();
        return services;
    }
}