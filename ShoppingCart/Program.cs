using Microsoft.EntityFrameworkCore;
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
}
else
{
    builder.Services.Configure<GrpcProductSettings>(builder.Configuration.GetSection("Grpc:Products"));
    builder.Services.Configure<UserSettings>(builder.Configuration.GetSection("Grpc:Users"));
    builder.Services.Configure<ShoppingCartKafkaSettings>(builder.Configuration.GetSection("Kafka:ShoppingCart"));
}


builder.Services.AddTransient<IProductCatalog, ProductsServiceClient>();
builder.Services.AddTransient<IShoppingCart, ShoppingCartService>();
builder.Services.AddSingleton<IKafkaProducer<CartDto>, ShoppingCartProducer<CartDto>>();
builder.Services.AddSingleton<UserClientService>();
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<DataContext>(o 
        => o.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnectionDev")));
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

app.UseCors(a => a.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();
app.MapControllers();
app.Run();