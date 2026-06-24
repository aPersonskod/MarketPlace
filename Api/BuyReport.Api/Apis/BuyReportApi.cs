using BuyReport.Application.Dtos;
using BuyReport.Application.Features.Commands;
using BuyReport.Application.Features.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BuyReport.Api.Apis;

public static class BuyReportApi
{
    public static IEndpointRouteBuilder MapBuyReportEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/buy-service").WithTags("BuyService");
        
        api.MapGet("/get-reports-by-userid", async (HttpContext context, ISender sender) =>
            {
                var token = context.GetAccessToken();
                if (token == null) return Results.Unauthorized();
                var query = new GetReportsQuery(token);
                return Results.Ok(await sender.Send(query));
            })
            .WithDescription("Get reports by userid")
            .WithName("GetReportsByUserId")
            .RequireAuthorization()
            .WithOpenApi();

        api.MapPost("/create-report", async (HttpContext context, ISender sender, [FromBody] CreateBuyReportDto createBuyReportDto) =>
            {
                var token = context.GetAccessToken();
                if (token == null) return Results.Unauthorized();
                var command = new CreateBuyReportCommand(createBuyReportDto.CartId, token);
                return Results.Ok(await sender.Send(command));
            })
            .WithDescription("Create report")
            .WithName("CreateReport")
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