using System.Reflection;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orchestrator.Application.Features.Commands;
using Orchestrator.Application.Interfaces;
using Orchestrator.Application.Saga.SagaDatas;
using Orchestrator.Application.Saga.SagaStateMachines;
using Orchestrator.Infrastructure.Data;
using Orchestrator.Infrastructure.Repositories;
using Orchestrator.Infrastructure.Settings;
using Shared.Infrastructure;

namespace Orchestrator.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddOrchestratorInfrastructure(this IServiceCollection services,
        IConfiguration configuration, IWebHostEnvironment environment)
    {
        //string? dbConnectionString;
        IConfigurationSection buyReportSettings;
        IConfigurationSection cartSettings;
        IConfigurationSection userSettings;
        if (environment.IsDevelopment())
        {
            buyReportSettings = configuration.GetSection("Grpc:BuyReportOptionsDev");
            cartSettings = configuration.GetSection("Grpc:CartOptionsDev");
            userSettings = configuration.GetSection("Grpc:UserOptionsDev");
            //dbConnectionString = Environment.GetEnvironmentVariable("PostgresConnectionDev");
        }
        else
        {
            buyReportSettings = configuration.GetSection("Grpc:BuyReportOptions");
            cartSettings = configuration.GetSection("Grpc:CartOptions");
            userSettings = configuration.GetSection("Grpc:UserOptions");
            //dbConnectionString = Environment.GetEnvironmentVariable("PostgresConnection");
        }
        services.AddAuthInfrastructure(configuration);
        services.AddScoped<IBuyReportRepository, BuyReportRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        // settings
        services.Configure<BuyReportSettings>(buyReportSettings);
        services.Configure<CartSettings>(cartSettings);
        services.Configure<UserSettings>(userSettings);
        // db context
        //services.AddDbContext<CartSagaDbContext>(o => o.UseNpgsql(dbConnectionString));
        services.AddMassTransit(x =>
        {
            x.AddConsumers(typeof(ConfirmCartCommandConsumer).Assembly);
            x.AddSagaStateMachine<CartStateMachine, CartStateSagaData>().InMemoryRepository();
            x.UsingInMemory((context, config) => config.ConfigureEndpoints(context));
        });
        return services;
    }
}