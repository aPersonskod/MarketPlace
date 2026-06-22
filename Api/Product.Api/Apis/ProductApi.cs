using Product.Application.Interfaces;

namespace Product.Api.Apis;

public static class ProductApi
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/product-service").WithTags("Product");
        
        api.MapGet("/get-all", async (IProductService productService) 
                => Results.Ok(await productService.Get()))
            .WithDescription("Get all products")
            .WithName("GetAllProducts")
            .WithOpenApi();

        api.MapGet("/{id:guid}", async (IProductService productService, Guid id) 
                => Results.Ok(await productService.Get(id)))
            .WithName("Get product by id")
            .WithName("GetProduct")
            .WithOpenApi();
        
        return app;
    }
}