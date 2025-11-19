using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Models;
using productServiceClient;

namespace ShoppingCart.Services;

public class ProductsServiceClient : IProductCatalog
{
    private const string Address = "https://localhost:7001";
    private readonly ProductService.ProductServiceClient _client;

    public ProductsServiceClient()
    {
        var channel = GrpcChannel.ForAddress(Address);
        _client = new ProductService.ProductServiceClient(channel);
    }
    
    public async Task<IEnumerable<Product>> Get()
    {
        try
        {
            var reply = await _client.GetAllAsync(new Empty());
            return await Task.FromResult(reply.Products.Select(x => new Product()
            {
                Id = Guid.Parse(x.Id),
                Name = x.Name
            }));
        }
        catch (RpcException e)
        {
            throw new RpcException(new Status(e.StatusCode, e.Status.Detail));
        }
    }

    public async Task<Product?> Get(Guid productId)
    {
        try
        {
            var reply = await _client.GetAsync(new GetProductRequest() { Id = productId.ToString() });
            return await Task.FromResult(new Product()
            {
                Id = Guid.Parse(reply.Id),
                Name = reply.Name
            });
        }
        catch (RpcException e)
        {
            throw new RpcException(new Status(e.StatusCode, e.Status.Detail));
        }
    }
}