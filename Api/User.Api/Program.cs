using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using User.Api.Middleware.Error;
using User.Application;
using User.Application.Dto;
using User.Application.Interfaces;
using User.Infrastructure;
using User.Infrastructure.Settings;

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

app.MapPost("/api/user-service/login", async (IUserService userService, IOptions<AuthSettings> authSettings,
        [FromBody]UserCredentialsDto credentialsDto) =>
    {
        var userDto = await userService.Authorize(credentialsDto);
        if (userDto == null) return Results.Unauthorized();
        var claims = new List<Claim>()
        {
            new Claim("Id", userDto.Id.ToString()),
            new Claim("Role", userDto.Role)
        };
        var jwt = new JwtSecurityToken(
            issuer: authSettings.Value.Issuer,
            audience: authSettings.Value.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: new SigningCredentials(authSettings.Value.SecurityKey, SecurityAlgorithms.HmacSha256)
        );
        var token = new JwtSecurityTokenHandler().WriteToken(jwt);
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
        var idStr = user.FindFirst("id")?.Value;
        if (!Guid.TryParse(idStr, out var id)) return Results.Unauthorized();
        return Results.Ok(await userService.Get());
    })
    .WithDescription("Get users")
    .WithName("GetUsers")
    .RequireAuthorization()
    .WithOpenApi();

app.MapGet("/api/user-service", async (ClaimsPrincipal user, IUserService userService) =>
    {
        var idStr = user.FindFirst("id")?.Value;
        if (!Guid.TryParse(idStr, out var id)) return Results.Unauthorized();
        return Results.Ok(await userService.Get(id));
    })
    .WithDescription("Get user by id")
    .WithName("GetUserById")
    .RequireAuthorization()
    .WithOpenApi();

app.MapDelete("/api/user-service", async (ClaimsPrincipal user, IUserService userService) =>
    {
        var idStr = user.FindFirst("id")?.Value;
        if (!Guid.TryParse(idStr, out var id)) return Results.Unauthorized();
        await userService.Delete(id);
        return Results.NoContent();
    })
    .WithDescription("Delete user")
    .WithName("DeleteUser")
    .RequireAuthorization()
    .WithOpenApi();

app.MapPatch("/api/user-service/top-up-money", async (ClaimsPrincipal user, IUserService userService, 
            [FromQuery] int money) =>
    {
        var idStr = user.FindFirst("id")?.Value;
        if (!Guid.TryParse(idStr, out var id)) return Results.Unauthorized();
        var userMoneyDto = new UserMoneyDto() { Id = id, Money = money };
        return Results.Ok(await userService.TopUpMoney(userMoneyDto));
    })
    .WithDescription("Money replenishment")
    .WithName("MoneyReplenishment")
    .RequireAuthorization()
    .WithOpenApi();

app.MapPatch("/api/user-service/spend-money", async (ClaimsPrincipal user, IUserService userService, 
            [FromBody] int money) =>
    {
        var idStr = user.FindFirst("id")?.Value;
        if (!Guid.TryParse(idStr, out var id)) return Results.Unauthorized();
        var userMoneyDto = new UserMoneyDto() { Id = id, Money = money };
        return Results.Ok(await userService.SpendMoney(userMoneyDto));
    })
    .WithDescription("Money spending")
    .WithName("MoneySpending")
    .RequireAuthorization()
    .WithOpenApi();

app.Run();