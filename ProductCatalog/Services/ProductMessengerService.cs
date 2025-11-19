using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using productServiceServer;

namespace ProductCatalog.Services;

public class ProductMessengerService(DataContext dataContext) : ProductService.ProductServiceBase
{
    public override Task<ListProductsReply> GetAll(Empty request, ServerCallContext context)
    {
        var products = dataContext.Products.Select(x => new ProductReply()
        {
            Id = x.Id.ToString(),
            Name = x.Name
        });
        var productsReply = new ListProductsReply();
        productsReply.Products.AddRange(products);
        return Task.FromResult(productsReply);
    }

    public override Task<ProductReply> Get(GetProductRequest request, ServerCallContext context)
    {
        var foundProduct = dataContext.Products.FirstOrDefault(x => x.Id == Guid.Parse(request.Id));
        if (foundProduct == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));
        }

        return Task.FromResult(new ProductReply()
        {
            Id = foundProduct.Id.ToString(),
            Name = foundProduct.Name
        });
    }
}