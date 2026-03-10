using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.Dtos;
using Models.Interfaces;
using ShoppingCart;
using ShoppingCart.Services;
using ShoppingCart.Settings;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddControllers();
if (builder.Environment.IsDevelopment())
{
    builder.Services.Configure<GrpcProductSettings>(builder.Configuration.GetSection("Grpc:ProductsDev"));
    builder.Services.Configure<UserSettings>(builder.Configuration.GetSection("Grpc:UsersDev"));
    builder.Services.Configure<ShoppingCartKafkaSettings>(builder.Configuration.GetSection("Kafka:ShoppingCartDev"));
    builder.Services.AddStackExchangeRedisCache(o =>
    {
        o.Configuration = builder.Configuration.GetValue<string>("Redis:ConfigurationDev");
        o.InstanceName = builder.Configuration.GetValue<string>("Redis:InstanceNameDev");
    });
    builder.Services.AddDbContext<DataContext>(o 
        => o.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnectionDev")));
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
}
else
{
    builder.Services.Configure<GrpcProductSettings>(builder.Configuration.GetSection("Grpc:Products"));
    builder.Services.Configure<UserSettings>(builder.Configuration.GetSection("Grpc:Users"));
    builder.Services.Configure<ShoppingCartKafkaSettings>(builder.Configuration.GetSection("Kafka:ShoppingCart"));
    builder.Services.AddDbContext<DataContext>(o 
        => o.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));
}

builder.Services.AddTransient<IProductCatalog, ProductsServiceClient>();
builder.Services.AddTransient<IShoppingCart, ShoppingCartService>();
builder.Services.AddScoped<IKafkaProducer<CartDto>, ShoppingCartProducer<CartDto>>();
builder.Services.AddScoped<UserClientService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(a => a.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();