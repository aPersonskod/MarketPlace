using BuyReport.Application.Interfaces;
using BuyReport.Infrastructure.Data;
using BuyReport.Infrastructure.Repositories;
using BuyReport.Infrastructure.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Infrastructure;

namespace BuyReport.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddBuyReportInfrastructure(this IServiceCollection services,
        IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddAuthInfrastructure(configuration);
        services.AddScoped<IBuyReportRepository, BuyReportRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        if (environment.IsDevelopment())
        {
            services.Configure<UserSettings>(configuration.GetSection("Grpc:UserOptionsDev"));
            services.Configure<CartSettings>(configuration.GetSection("Grpc:CartOptionsDev"));
            services.AddDbContext<AppDbContext>(o => o.UseNpgsql(configuration.GetConnectionString("PostgresConnectionDev")));
        }
        else
        {
            services.Configure<UserSettings>(configuration.GetSection("Grpc:UserOptions"));
            services.Configure<CartSettings>(configuration.GetSection("Grpc:CartOptions"));
            services.AddDbContext<AppDbContext>(o => o.UseNpgsql(configuration.GetConnectionString("PostgresConnection")));
        }
        return services;
    }
}