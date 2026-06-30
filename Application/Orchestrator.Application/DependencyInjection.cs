using Microsoft.Extensions.DependencyInjection;

namespace Orchestrator.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddOrchestratorApplication(this IServiceCollection services)
    {
        return services;
    }
}