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
    private readonly ILogger<ProductsServiceClient> _logger;

    public ProductsServiceClient(IOptions<GrpcProductSettings> grpcOptions, ILogger<ProductsServiceClient> logger)
    {
        _logger = logger;
        _logger.LogInformation($"Product client service {grpcOptions.Value.HttpsAddress}");
        var httpHandler = new HttpClientHandler();
        // Return true to allow certificates that are untrusted/invalid
        httpHandler.ServerCertificateCustomValidationCallback = 
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        var channel = GrpcChannel.ForAddress(grpcOptions.Value.HttpsAddress, 
            new GrpcChannelOptions { HttpHandler = httpHandler });
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
            _logger.LogInformation($"Try to get product by id: {productId}");
            var reply = await _client.GetAsync(new GetProductRequest() { Id = productId.ToString() });
            _logger.LogInformation($"product name: {reply.Name}");
            return await Task.FromResult(new ProductDto()
            {
                Id = Guid.Parse(reply.Id),
                Name = reply.Name,
                Cost = reply.Cost
            });
        }
        catch (RpcException e)
        {
            _logger.LogError($"product service exception: {e.Status.Detail}");
            throw new RpcException(new Status(e.StatusCode, e.Status.Detail));
        }
    }
}