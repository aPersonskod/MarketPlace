using Cart.Application;
using Cart.Application.Interfaces;
using Cart.Application.Interfaces.Repositories;
using Cart.Infrastructure.Data;
using Cart.Infrastructure.Repositories;
using Cart.Infrastructure.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Infrastructure;

namespace Cart.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCartInfrastructure(this IServiceCollection services, 
        IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddAuthInfrastructure(configuration);
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<CartRepository>();
        services.AddScoped<CachedCartRepository>();
        services.AddScoped<CartRepositoryResolver>(s => key => key switch
        {
            CartRepositoryKeys.Cart => s.GetService<CartRepository>(),
            CartRepositoryKeys.CachedCart => s.GetService<CachedCartRepository>(),
            _ => throw new KeyNotFoundException("Unknown cart repository")
        });
        services.AddScoped<OrderRepository>();
        services.AddScoped<CachedOrderRepository>();
        services.AddScoped<OrderRepositoryResolver>(s => key => key switch
        {
            OrderRepositoryKeys.Order => s.GetService<OrderRepository>(),
            OrderRepositoryKeys.CachedOrder => s.GetService<CachedOrderRepository>(),
            _ => throw new KeyNotFoundException("Unknown order repository")
        });
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IPlaceRepository, PlaceRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IBuyReportRepository, BuyReportRepository>();
        services.AddStackExchangeRedisCache(o =>
        {
            o.Configuration = configuration.GetValue<string>("Redis:ConfigurationDev");
            o.InstanceName = configuration.GetValue<string>("Redis:InstanceNameDev");
        });
        if (environment.IsDevelopment())
        {
            services.Configure<GrpcProductSettings>(configuration.GetSection("Grpc:ProductsDev"));
            services.AddDbContext<AppDbContext>(o => o.UseNpgsql(configuration.GetConnectionString("PostgresConnectionDev")));
        }
        else
        {
            services.Configure<GrpcProductSettings>(configuration.GetSection("Grpc:Products"));
            services.AddDbContext<AppDbContext>(o => o.UseNpgsql(configuration.GetConnectionString("PostgresConnection")));
        }
        return services;
    }
}