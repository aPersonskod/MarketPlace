using Microsoft.EntityFrameworkCore;
using Models;
using Models.Interfaces;
using ProductCatalog;
using ProductCatalog.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddGrpc();

builder.Services.AddDbContext<DataContext>(o 
    => o.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

builder.Services.AddTransient<IProductCatalog, ProductCatalogService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGrpcService<ProductMessengerService>();

app.UseCors(a => a.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
