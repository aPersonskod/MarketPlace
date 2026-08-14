using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Product.Application.Interfaces;
using Product.Infrastructure.Data;
using Product.Infrastructure.Repositories;

namespace Product.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddProductInfrastructure(this IServiceCollection services, 
        IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddGrpc();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddStackExchangeRedisCache(o =>
        {
            o.Configuration = configuration.GetValue<string>("Redis:ConfigurationDev");
            o.InstanceName = configuration.GetValue<string>("Redis:InstanceNameDev");
        });
        if (environment.IsDevelopment())
        {
            // todo ОБЯЗАТЕЛЬНО ПРОВЕРЬ ПРИ ЗАПУСКЕ ДОККЕРА !!!
            var isRunningFromContainer = true;
            if (isRunningFromContainer)
            {
                // need that because in docker-compose env is development
                services.AddDbContext<AppDbContext>(o => o.UseNpgsql(configuration.GetConnectionString("PostgresConnection")));
            }
            else
            {
                services.AddDbContext<AppDbContext>(o => o.UseNpgsql(configuration.GetConnectionString("PostgresConnectionDev")));
            }
        }
        else
        {
            services.AddDbContext<AppDbContext>(o => o.UseNpgsql(configuration.GetConnectionString("PostgresConnection")));
        }
        return services;
    }
}