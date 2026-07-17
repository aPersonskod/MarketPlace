using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using User.Application.Dto;
using User.Application.Interfaces;

namespace User.Api.Apis;

public static class UserApi
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/user-service").WithTags("User");

        api.MapPost("", async (IUserService userService, [FromBody] CreateUserDto createUserDto)
                => Results.Ok(await userService.Add(createUserDto)))
            .WithDescription("Create new user")
            .WithName("CreateUser")
            .WithOpenApi();

        api.MapGet("/get-all", async (ClaimsPrincipal user, IUserService userService) =>
            {
                var idStr = user.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                if (!Guid.TryParse(idStr, out _)) return Results.Unauthorized();
                return Results.Ok(await userService.Get());
            })
            .WithDescription("Get users")
            .WithName("GetUsers")
            .RequireAuthorization(new AuthorizeAttribute { Roles = "admin" })
            .WithOpenApi();

        api.MapGet("", async (ClaimsPrincipal user, IUserService userService) =>
            {
                var idStr = user.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                if (!Guid.TryParse(idStr, out var id)) return Results.Unauthorized();
                return Results.Ok(await userService.Get(id));
            })
            .WithDescription("Get user by id")
            .WithName("GetUserById")
            .RequireAuthorization()
            .WithOpenApi();

        api.MapDelete("", async (ClaimsPrincipal user, IUserService userService) =>
            {
                var idStr = user.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                if (!Guid.TryParse(idStr, out var id)) return Results.Unauthorized();
                await userService.Delete(id);
                return Results.NoContent();
            })
            .WithDescription("Delete user")
            .WithName("DeleteUser")
            .RequireAuthorization()
            .WithOpenApi();

        api.MapPatch("/top-up-money", async (ClaimsPrincipal user, IUserService userService,
                [FromBody] MoneyDto moneyDto) =>
            {
                var idStr = user.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                if (!Guid.TryParse(idStr, out var id)) return Results.Unauthorized();
                var userMoneyDto = new UserMoneyDto() { UserId = id, Money = moneyDto.Money };
                return Results.Ok(await userService.TopUpMoney(userMoneyDto));
            })
            .WithDescription("Money replenishment")
            .WithName("MoneyReplenishment")
            .RequireAuthorization()
            .WithOpenApi();

        api.MapPatch("/spend-money", async (ClaimsPrincipal user, IUserService userService,
                [FromBody] MoneyDto moneyDto) =>
            {
                var idStr = user.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                if (!Guid.TryParse(idStr, out var id)) return Results.Unauthorized();
                var userMoneyDto = new UserMoneyDto() { UserId = id, Money = moneyDto.Money };
                return Results.Ok(await userService.SpendMoney(userMoneyDto));
            })
            .WithDescription("Money spending")
            .WithName("MoneySpending")
            .RequireAuthorization()
            .WithOpenApi();
        
        return app;
    }
}