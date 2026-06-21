using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Product.Infrastructure.Data;
using productServiceServer;

namespace Product.Infrastructure.Services;

public class ProductMessengerService(AppDbContext dbContext) : ProductService.ProductServiceBase
{
    public override async Task<ProductReply> Get(GetProductRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.Id, out var id)) 
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid id"));
        var foundProduct = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
        if (foundProduct == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));
        }
        return new ProductReply()
        {
            Id = foundProduct.Id.ToString(),
            Name = foundProduct.Name,
            Cost = foundProduct.Cost,
        };
    }
}