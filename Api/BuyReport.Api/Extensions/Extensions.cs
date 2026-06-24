using BuyReport.Api.Middleware.Error;

namespace BuyReport.Api.Extensions;

public static class Extensions
{
    public static IServiceCollection AddDefaultServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddCors(o =>
            o.AddPolicy("CorsPolicy", b =>
            {
                b.AllowAnyMethod()
                    .SetIsOriginAllowed(_ => true)
                    .AllowAnyHeader()
                    .AllowCredentials();
            }));
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        return services;
    }
    
    public static IApplicationBuilder UseDefaultApi(this IApplicationBuilder app)
    {
        app.UseHttpsRedirection();
        app.UseCors("CorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseExceptionHandler();
        return app;
    }
}