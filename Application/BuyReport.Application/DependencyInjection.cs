using System.Reflection;
using BuyReport.Application.PiplineBehaviours;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BuyReport.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddBuyReportApplication(this IServiceCollection services)
    {
        services.AddMediatR(c => {
            c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            c.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        // Register all validators automatically from the assembly
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }
}