using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orchestrator.Application.Features.Activities;
using Orchestrator.Application.Interfaces;
using Orchestrator.Infrastructure.Repositories;
using Orchestrator.Infrastructure.Settings;
using Shared.Infrastructure;

namespace Orchestrator.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddOrchestratorInfrastructure(this IServiceCollection services,
        IConfiguration configuration, IWebHostEnvironment environment)
    {
        IConfigurationSection buyReportSettings;
        IConfigurationSection cartSettings;
        IConfigurationSection userSettings;
        if (environment.IsDevelopment())
        {
            buyReportSettings = configuration.GetSection("Grpc:BuyReportOptionsDev");
            cartSettings = configuration.GetSection("Grpc:CartOptionsDev");
            userSettings = configuration.GetSection("Grpc:UserOptionsDev");
        }
        else
        {
            buyReportSettings = configuration.GetSection("Grpc:BuyReportOptions");
            cartSettings = configuration.GetSection("Grpc:CartOptions");
            userSettings = configuration.GetSection("Grpc:UserOptions");
        }
        // settings
        services.Configure<BuyReportSettings>(buyReportSettings);
        services.Configure<CartSettings>(cartSettings);
        services.Configure<UserSettings>(userSettings);
        services.AddAuthInfrastructure(configuration);
        services.AddScoped<IBuyReportRepository, BuyReportRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddMassTransit(x =>
        {
            x.AddActivities(typeof(ConfirmCartActivity).Assembly);
            //x.UsingInMemory((context, config) => config.ConfigureEndpoints(context));
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("admin");
                    h.Password("securepassword123");
                });
                cfg.ConfigureEndpoints(context);
            });
        });
        return services;
    }
}