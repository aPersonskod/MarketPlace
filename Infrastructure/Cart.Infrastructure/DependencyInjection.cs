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

namespace Cart.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddProductInfrastructure(this IServiceCollection services, 
        IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IPlaceRepository, PlaceRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
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