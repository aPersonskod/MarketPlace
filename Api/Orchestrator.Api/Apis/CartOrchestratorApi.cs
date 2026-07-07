using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Orchestrator.Application.Dtos;
using Orchestrator.Application.Features.Events;

namespace Orchestrator.Api.Apis;

public static class CartOrchestratorApi
{
    public static IEndpointRouteBuilder MapBuyOrchestratorEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/buy-actions").WithTags("BuyActions");

        api.MapPost("/buy-cart", async (HttpContext context, IPublishEndpoint publishEndpoint,
                [FromBody] CartSubmittedDto cartSubmittedDto) =>
            {
                var token = context.GetAccessToken();
                if (token == null) return Results.Unauthorized();
                await publishEndpoint.Publish(new CartSubmittedEvent(cartSubmittedDto.CartId, cartSubmittedDto.PlaceId, token));
                return Results.Ok();
            })
            .WithDescription("Buy cart saga")
            .WithName("BuyCart")
            .RequireAuthorization()
            .WithOpenApi();

        return app;
    }

    private static string? GetAccessToken(this HttpContext context)
    {
        string? authHeader = context.Request.Headers.Authorization;
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return authHeader.Substring("Bearer ".Length).Trim();
    }
}