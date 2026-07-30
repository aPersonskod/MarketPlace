using Cart.Application;
using Cart.Application.Interfaces;
using Cart.Application.Interfaces.Repositories;
using Cart.Application.Interfaces.Repositories.Cached;
using Cart.Infrastructure.Data;
using Cart.Infrastructure.Repositories;
using Cart.Infrastructure.Repositories.Cached;
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
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<ICachedCartRepository, CachedCartRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ICachedOrderRepository, CachedOrderRepository>();
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