using BuyActions;
using BuyActions.Services;
using BuyActions.Settings;
using Models;
using Models.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddCors();

builder.Services.Configure<UserSettings>(builder.Configuration.GetSection("Grpc:Users"));
builder.Services.Configure<ShoppingCartSettings>(builder.Configuration.GetSection("Grpc:ShoppingCarts"));

builder.Services.AddSingleton<ShoppingCartClientService>();
builder.Services.AddSingleton<UserClientService>();
builder.Services.AddTransient<IBuyService, BuyService>();
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