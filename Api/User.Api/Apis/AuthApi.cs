using Microsoft.AspNetCore.Mvc;
using User.Application.Dto;
using User.Application.Interfaces;

namespace User.Api.Apis;

public static class AuthApi
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/user-service").WithTags("Auth");

        api.MapPost("/login", async (IAuthService authService, [FromBody] UserCredentialsDto credentialsDto) =>
            {
                var tokenData = await authService.Authorize(credentialsDto);
                return Results.Ok(tokenData);
            })
            .WithDescription("User authorization")
            .WithName("UserAuthorization")
            .WithOpenApi();

        api.MapPost("/refresh",
                async (IAuthService authService, [FromBody] TokensData tokenData) =>
                {
                    var newTokenData = await authService.Refresh(tokenData);
                    return Results.Ok(newTokenData);
                })
            .WithDescription("User refresh token")
            .WithName("UserRefreshToken")
            .WithOpenApi();

        return app;
    }
}