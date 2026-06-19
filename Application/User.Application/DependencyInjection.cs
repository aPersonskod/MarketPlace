using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using User.Application.Interfaces;
using User.Application.Services;
using User.Application.Validations;

namespace User.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddUserApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<MoneyDtoValidator>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}