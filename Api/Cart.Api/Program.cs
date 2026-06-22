using Cart.Api.Apis;
using Cart.Api.Extensions;
using Cart.Application;
using Cart.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDefaultServices();
builder.Services.AddCartInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddCartApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultApi();
app.MapCartEndpoints();
app.MapOrderEndpoints();
app.MapPlaceEndpoints();
app.Run();