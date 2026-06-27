using System.Security.Claims;
using Cart.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructure.Extensions;

namespace Cart.Api.Apis;

public static class CartApi
{
    public static IEndpointRouteBuilder MapCartEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/cart-service").WithTags("Cart");

        api.MapGet("/get-bought-carts", async (ClaimsPrincipal user, ICartService cartService) =>
            {
                var credentials = user.GetAuthCredentials();
                return credentials == null
                    ? Results.Unauthorized()
                    : Results.Ok(await cartService.GetBoughtCartsAsync(credentials.Value.UserId));
            })
            .WithDescription("Get bought carts")
            .WithName("GetBoughtCarts")
            .RequireAuthorization()
            .WithOpenApi();

        api.MapGet("/get-cart", async (ClaimsPrincipal user, ICartService cartService) =>
            {
                var credentials = user.GetAuthCredentials();
                return credentials == null
                    ? Results.Unauthorized()
                    : Results.Ok(await cartService.GetCartByUserIdAsync(credentials.Value.UserId));
            })
            .WithDescription("Get cart")
            .WithName("GetCart")
            .RequireAuthorization()
            .WithOpenApi();
        
        api.MapGet("/is-cart-exist/{cartId:guid}", async (ICartService cartService, Guid cartId) 
                => Results.Ok(await cartService.GetCartByIdAsync(cartId)))
            .WithDescription("Get cart by id")
            .WithName("GetCartById")
            .RequireAuthorization()
            .WithOpenApi();

        api.MapDelete("/delete-cart/{cartId:guid}", async (ICartService cartService, Guid cartId) =>
            {
                await cartService.DeleteCartAsync(cartId);
                return Results.NoContent();
            })
            .WithDescription("Delete cart")
            .WithName("DeleteCart")
            .RequireAuthorization()
            .WithOpenApi();

        api.MapPatch("/confirm-cart", async (ClaimsPrincipal user, ICartService cartService, [FromQuery] Guid placeId) =>
            {
                var credentials = user.GetAuthCredentials();
                return credentials == null
                    ? Results.Unauthorized()
                    : Results.Ok(await cartService.ConfirmCartAsync(credentials.Value.UserId, placeId));
            })
            .WithDescription("Confirm cart")
            .WithName("ConfirmCart")
            .RequireAuthorization()
            .WithOpenApi();

        api.MapPatch("/unconfirm-cart", async (ClaimsPrincipal user, ICartService cartService) =>
            {
                var credentials = user.GetAuthCredentials();
                return credentials == null
                    ? Results.Unauthorized()
                    : Results.Ok(await cartService.UnConfirmCartAsync(credentials.Value.UserId));
            })
            .WithDescription("Unconfirm cart")
            .WithName("UnconfirmCart")
            .RequireAuthorization()
            .WithOpenApi();

        api.MapPatch("/mark-cart-as-bought/{cartId:guid}", async (ICartService cartService, Guid cartId) =>
            {
                await cartService.MarkCartAsBoughtAsync(cartId);
                return Results.Ok();
            })
            .WithDescription("Mark cart as bought")
            .WithName("MarkCartAsBought")
            .RequireAuthorization()
            .WithOpenApi();

        api.MapPatch("/mark-cart-as-not-bought/{cartId:guid}", async (ICartService cartService, Guid cartId) =>
            {
                await cartService.MarkCartAsNotBoughtAsync(cartId);
                return Results.Ok();
            })
            .WithDescription("Mark cart as not bought")
            .WithName("MarkCartAsNotBought")
            .RequireAuthorization()
            .WithOpenApi();

        return app;
    }
}