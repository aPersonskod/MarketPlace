using Product.Api.Middleware.Error;
using Product.Application;
using Product.Application.Interfaces;
using Product.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProductInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddProductApplication();
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
app.UseExceptionHandler();

app.MapGet("/api/product-service/get-all", async (IProductService productService) 
    => Results.Ok(await productService.Get()))
    .WithDescription("Get all products")
    .WithName("GetAllProducts")
    .WithOpenApi();

app.MapGet("/api/product-service/{id:guid}", async (IProductService productService, Guid id) 
    => Results.Ok(await productService.Get(id)))
    .WithName("Get product by id")
    .WithName("GetProduct")
    .WithOpenApi();

app.Run();