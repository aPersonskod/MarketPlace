using Cart.Application.Interfaces.Repositories;
using Cart.Infrastructure.Settings;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Product.Application.Dtos;
using productServiceClient;
using ProductService = productServiceClient.ProductService;

namespace Cart.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ILogger<ProductRepository> _logger;
    private readonly ProductService.ProductServiceClient _client;
    public ProductRepository(IOptions<GrpcProductSettings> grpcOptions, ILogger<ProductRepository> logger)
    {
        _logger = logger;
        var httpHandler = new HttpClientHandler();
        // Return true to allow certificates that are untrusted/invalid
        httpHandler.ServerCertificateCustomValidationCallback = 
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        var channel = GrpcChannel.ForAddress(grpcOptions.Value.HttpsAddress, 
            new GrpcChannelOptions { HttpHandler = httpHandler });
        _client = new ProductService.ProductServiceClient(channel);
    }
    public async Task<ProductDto> GetProductByIdAsync(Guid productId)
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