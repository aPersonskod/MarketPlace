using System.Security.Claims;
using Cart.Application.Interfaces.Services;
using Shared.Infrastructure.Extensions;

namespace Cart.Api.Apis;

public static class BuyReportApi
{
    public static IEndpointRouteBuilder MapBuyReportEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/cart-service").WithTags("BuyReport");
        
        api.MapGet("/get-carts-for-report", async (ClaimsPrincipal user, IBuyReportService buyReportService) =>
            {
                var credentials = user.GetAuthCredentials();
                return credentials == null 
                    ? Results.Unauthorized() 
                    : Results.Ok(await buyReportService.GetCartsForReportAsync(credentials.Value.UserId));
            })
            .WithDescription("Get detailed cart buy report")
            .WithName("GetCartBuyReport")
            .RequireAuthorization()
            .WithOpenApi();
        
        return app;
    }
}