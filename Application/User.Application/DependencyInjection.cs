using Microsoft.Extensions.DependencyInjection;
using User.Application.Interfaces;
using User.Application.Services;

namespace User.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddUserApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}