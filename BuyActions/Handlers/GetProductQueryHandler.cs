using BuyActions.Queries;
using BuyActions.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using Models.Dtos;

namespace BuyActions.Handlers;

public class GetProductQueryHandler(IOptions<ProductCatalogSettings> productOptions)
    : IRequestHandler<GetProductQuery, ProductDto?>
{
    public async Task<ProductDto?> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var query = $"{productOptions.Value.Address}/{request.ProductId}";
        // this handles https
        var clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
        using var client = new HttpClient(clientHandler);
        var httpResponse = await client.GetAsync(query, cancellationToken);

        if (!httpResponse.IsSuccessStatusCode) return await Task.FromResult<ProductDto?>(null);
        var response = await httpResponse.Content.ReadFromJsonAsync<ProductDto>(cancellationToken);
        return await Task.FromResult(response);
    }
}