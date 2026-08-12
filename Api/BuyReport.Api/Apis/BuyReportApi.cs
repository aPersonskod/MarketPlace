using System.Security.Claims;
using BuyReport.Application.Dtos;
using BuyReport.Application.Features.Commands;
using BuyReport.Application.Features.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Infrastructure.Extensions;

namespace BuyReport.Api.Apis;

public static class BuyReportApi
{
    public static IEndpointRouteBuilder MapBuyReportEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/buy-service").WithTags("BuyService");
        
        api.MapGet("/get-reports-by-userid", async (HttpContext context, ISender sender, ClaimsPrincipal user,
                [FromQuery]int pageNumber = 1, [FromQuery]int pageSize = 6) =>
            {
                var token = context.GetAccessToken();
                if (token == null) return Results.Unauthorized();
                var credentials = user.GetAuthCredentials();
                if (credentials == null) return Results.Unauthorized();
                
                if (pageNumber < 1) pageNumber = 1;
                if (pageSize < 1) pageSize = 6;
                
                var query = new GetReportsQuery(credentials.Value.UserId, token, pageNumber, pageSize);
                return Results.Ok(await sender.Send(query));
            })
            .WithDescription("Get reports by userid")
            .WithName("GetReportsByUserId")
            .RequireAuthorization()
            .WithOpenApi();

        api.MapPost("/create-report", async (HttpContext context, ISender sender, ClaimsPrincipal user,
                [FromBody] CreateBuyReportDto createBuyReportDto) =>
            {
                var token = context.GetAccessToken();
                if (token == null) return Results.Unauthorized();
                var credentials = user.GetAuthCredentials();
                if (credentials == null) return Results.Unauthorized();
                var command = new CreateBuyReportCommand(createBuyReportDto.CartId, credentials.Value.UserId, token);
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