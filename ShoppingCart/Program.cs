using Models;
using Models.Interfaces;
using ShoppingCart;
using ShoppingCart.Services;
using ShoppingCart.Settings;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddControllers();

builder.Services.Configure<GrpcProductSettings>(builder.Configuration.GetSection("Grpc:Products"));
builder.Services.Configure<UserSettings>(builder.Configuration.GetSection("Grpc:Users"));
builder.Services.Configure<ShoppingCartKafkaSettings>(builder.Configuration.GetSection("Kafka:ShoppingCart"));

builder.Services.AddTransient<IProductCatalog, ProductsServiceClient>();
builder.Services.AddTransient<IShoppingCart, ShoppingCartService>();
builder.Services.AddSingleton<IKafkaProducer<Cart>, ShoppingCartProducer<Cart>>();
builder.Services.AddSingleton<UserClientService>();
builder.Services.AddSingleton<DataContext>();

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