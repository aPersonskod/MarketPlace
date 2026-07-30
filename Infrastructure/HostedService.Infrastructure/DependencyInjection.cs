using HostedService.Application.Interfaces.Repositories;
using HostedService.Infrastructure.Repositories;
using HostedService.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HostedService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddHostedServiceInfrastructure(this IServiceCollection services, 
        IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddSingleton<ICartRepository, CartRepository>();
        services.AddSingleton<IOrderRepository, OrderRepository>();
        services.AddStackExchangeRedisCache(o =>
        {
            o.Configuration = configuration.GetValue<string>("Redis:ConfigurationDev");
            o.InstanceName = configuration.GetValue<string>("Redis:InstanceNameDev");
        });
        var cartSettings = environment.IsDevelopment() ? "Grpc:CartOptionsDev" : "Grpc:CartOptions";
        services.Configure<CartSettings>(configuration.GetSection(cartSettings));
        return services;
    }
}