using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models.Interfaces;
using UserManipulations;
using UserManipulations.Services;
using UserManipulations.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(o => 
    o.AddPolicy("CorsPolicy", b =>
    {
        b.AllowAnyMethod()
            .SetIsOriginAllowed(_ => true)
            .AllowAnyHeader()
            .AllowCredentials();
    }));
builder.Services.AddControllers();
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("Auth"));
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(o => o.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Auth:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Auth:Audience"],
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Auth:Key"]!)),
        ValidateIssuerSigningKey = true
    });
builder.Services.AddAuthorization();

builder.Services.AddTransient<IUserManipulations, UserManipulationsService>();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<DataContext>(o 
        => o.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnectionDev")));
    builder.Services.AddStackExchangeRedisCache(o =>  
    {  
        o.Configuration = builder.Configuration.GetValue<string>("Redis:ConfigurationDev");  
        o.InstanceName = builder.Configuration.GetValue<string>("Redis:InstanceNameDev");  
    });
}
else
{
    builder.Services.AddDbContext<DataContext>(o 
        => o.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();