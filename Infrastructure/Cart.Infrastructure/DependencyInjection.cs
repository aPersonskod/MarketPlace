using System.Text;
using Cart.Application.Interfaces;
using Cart.Application.Interfaces.Repositories;
using Cart.Infrastructure.Data;
using Cart.Infrastructure.Repositories;
using Cart.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Cart.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCartInfrastructure(this IServiceCollection services, 
        IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.Configure<AuthSettings>(configuration.GetSection("Auth"));
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Auth:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["Auth:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Auth:Key"]!)),
                    ValidateIssuerSigningKey = true
                };
                
                // Debugging Hook
                o.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"Auth failed: {context.Exception.Message}");
                        return Task.CompletedTask;
                    }
                };
            });
        services.AddAuthorization();
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