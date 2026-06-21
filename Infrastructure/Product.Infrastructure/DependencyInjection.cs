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
        services.AddScoped<IProductRepository, ProductRepository>();
        if (environment.IsDevelopment())
        {
            //services.AddDbContext<AppDbContext>(o => o.UseNpgsql(configuration.GetConnectionString("PostgresConnectionDev")));
            services.AddDbContext<AppDbContext>(o => o.UseNpgsql(configuration.GetConnectionString("PostgresConnection")));
        }
        else
        {
            services.AddDbContext<AppDbContext>(o => o.UseNpgsql(configuration.GetConnectionString("PostgresConnection")));
        }
        return services;
    }
}