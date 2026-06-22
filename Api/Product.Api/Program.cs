using Product.Api.Apis;
using Product.Api.Extensions;
using Product.Application;
using Product.Infrastructure;
using Product.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDefaultServices();
builder.Services.AddProductInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddProductApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultApi();
app.MapGrpcService<ProductMessengerService>();
app.MapProductEndpoints();
app.Run();