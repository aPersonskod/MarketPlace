using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;
using Models.Dtos;
using Models.Interfaces;
using productServiceClient;
using ShoppingCart.Settings;

namespace ShoppingCart.Services;

public class ProductsServiceClient : IProductCatalog
{
    private readonly ProductService.ProductServiceClient _client;

    public ProductsServiceClient(IOptions<GrpcProductSettings> grpcOptions)
    {
        var channel = GrpcChannel.ForAddress(grpcOptions.Value.Address);
        _client = new ProductService.ProductServiceClient(channel);
    }
    
    public async Task<IEnumerable<ProductDto>> Get()
    {
        try
        {
            var reply = await _client.GetAllAsync(new Empty());
            return await Task.FromResult(reply.Products.Select(x => new ProductDto()
            {
                Id = Guid.Parse(x.Id),
                Name = x.Name,
                Cost = x.Cost
            }));
        }
        catch (RpcException e)
        {
            throw new RpcException(new Status(e.StatusCode, e.Status.Detail));
        }
    }

    public async Task<ProductDto> Get(Guid productId)
    {
        try
        {
            var reply = await _client.GetAsync(new GetProductRequest() { Id = productId.ToString() });
            return await Task.FromResult(new ProductDto()
            {
                Id = Guid.Parse(reply.Id),
                Name = reply.Name,
                Cost = reply.Cost
            });
        }
        catch (RpcException e)
        {
            throw new RpcException(new Status(e.StatusCode, e.Status.Detail));
        }
    }
}