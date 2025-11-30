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

builder.Services.Configure<GrpcSettings>(builder.Configuration.GetSection("Grpc:Products"));

builder.Services.AddTransient<IProductCatalog, ProductsServiceClient>();
builder.Services.AddTransient<IShoppingCart, ShoppingCartService>();
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