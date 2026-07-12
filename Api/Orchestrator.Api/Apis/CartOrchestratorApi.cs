using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Orchestrator.Application.Dtos;
using Orchestrator.Application.Features.Events;
using Orchestrator.Application.Saga.SagaStateMachines;

namespace Orchestrator.Api.Apis;

public static class CartOrchestratorApi
{
    public static IEndpointRouteBuilder MapBuyOrchestratorEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/buy-actions").WithTags("BuyActions");

        api.MapPost("/buy-cart", async (HttpContext context, IBus bus, [FromBody] CartSubmittedDto cartSubmittedDto) =>
            {
                var token = context.GetAccessToken();
                if (token == null) return Results.Unauthorized();
                var submittedEvent = new CartSubmittedEvent(cartSubmittedDto.CartId, cartSubmittedDto.PlaceId, token);
                var saga = new SagaExecutor(bus);
                await saga.Execute(submittedEvent);
                return Results.Ok("Cart submitted successfully");
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