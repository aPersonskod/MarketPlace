using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User.Api.Middleware.Error;
using User.Application;
using User.Application.Dto;
using User.Application.Interfaces;
using User.Infrastructure;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddUserInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddUserApplication();
builder.Services.AddCors(o =>
    o.AddPolicy("CorsPolicy", b =>
    {
        b.AllowAnyMethod()
            .SetIsOriginAllowed(_ => true)
            .AllowAnyHeader()
            .AllowCredentials();
    }));
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler();

app.MapPost("/api/user-service/login", async (IUserService userService, [FromBody] UserCredentialsDto credentialsDto) =>
    {
        var token = await userService.Authorize(credentialsDto);
        return Results.Ok(token);
    })
    .WithDescription("User authorization")
    .WithName("UserAuthorization")
    .WithOpenApi();

app.MapPost("/api/user-service", async (IUserService userService, [FromBody] CreateUserDto createUserDto)
        => Results.Ok(await userService.Add(createUserDto)))
    .WithDescription("Create new user")
    .WithName("CreateUser")
    .WithOpenApi();

app.MapGet("/api/user-service/get-all", async (ClaimsPrincipal user, IUserService userService) =>
    {
        var idStr = user.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
        if (!Guid.TryParse(idStr, out _)) return Results.Unauthorized();
        return Results.Ok(await userService.Get());
    })
    .WithDescription("Get users")
    .WithName("GetUsers")
    .RequireAuthorization(new AuthorizeAttribute { Roles = "admin" })
    .WithOpenApi();

app.MapGet("/api/user-service", async (ClaimsPrincipal user, IUserService userService) =>
    {
        var idStr = user.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
        if (!Guid.TryParse(idStr, out var id)) return Results.Unauthorized();
        return Results.Ok(await userService.Get(id));
    })
    .WithDescription("Get user by id")
    .WithName("GetUserById")
    .RequireAuthorization(new AuthorizeAttribute { Roles = "admin, user" })
    .WithOpenApi();

app.MapDelete("/api/user-service", async (ClaimsPrincipal user, IUserService userService) =>
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

app.MapPatch("/api/user-service/top-up-money", async (ClaimsPrincipal user, IUserService userService,
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

app.MapPatch("/api/user-service/spend-money", async (ClaimsPrincipal user, IUserService userService,
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

app.Run();