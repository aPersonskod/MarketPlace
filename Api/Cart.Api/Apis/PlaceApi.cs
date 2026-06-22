using Cart.Application.Interfaces.Services;

namespace Cart.Api.Apis;

public static class PlaceApi
{
    public static IEndpointRouteBuilder MapPlaceEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/cart-service").WithTags("Place");
        
        api.MapGet("/get-places", async (IPlaceService placeService)
                => Results.Ok(await placeService.GetAllPlacesAsync()))
            .WithDescription("Get places")
            .WithName("GetPlaces")
            .WithOpenApi();
        
        api.MapGet("/get-place/{placeId:guid}", async (IPlaceService placeService, Guid placeId)
                => Results.Ok(await placeService.GetPlaceAsync(placeId)))
            .WithDescription("Get place by id")
            .WithName("GetPlaceById")
            .WithOpenApi();
        
        return app;
    }
}