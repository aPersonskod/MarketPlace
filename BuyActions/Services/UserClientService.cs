using System.Net;
using System.Runtime.CompilerServices;
using BuyActions.Settings;
using Microsoft.Extensions.Options;
using Models.Dtos;
using Models.Extensions;
using Models.Interfaces;

namespace BuyActions.Services;

public class UserClientService(IOptions<UserSettings> userOptions)
{
    private readonly string _baseAddress = userOptions.Value.Address;
    public async Task<UserDto> GetUser(Guid userId)
    {
        var query = $"{_baseAddress}/{userId}";
        return (await query.GetQuery<UserDto>())!;
    }

    public async Task<UserDto> SpendMoney(Guid userId, int money)
    {
        var query = $"{_baseAddress}/SpendMoney?userId={userId}&money={money}";
        return (await query.PostQuery<UserDto>())!;
    }
}

public class ProductCatalogClientService(IOptions<ProductCatalogSettings> productOptions) : IProductCatalog
{
    private readonly string _baseAddress = productOptions.Value.Address;
    public async Task<IEnumerable<ProductDto>> Get() => (await _baseAddress.GetQuery<IEnumerable<ProductDto>>())!;
    public async Task<ProductDto> Get(Guid id)
    {
        var query = $"{_baseAddress}/{id}";
        var response = new ProductDto();
        
        var clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
        using var client = new HttpClient(clientHandler);
        HttpResponseMessage async = await client.GetAsync(query);
        
        if (async.IsSuccessStatusCode)
        {
            response = await async.Content.ReadFromJsonAsync<ProductDto>();
        }
        else
        {
            /*var interpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 1);
            interpolatedStringHandler.AppendLiteral("server error code ");
            interpolatedStringHandler.AppendFormatted<HttpStatusCode>(async.StatusCode);
            throw new ArgumentNullException(interpolatedStringHandler.ToStringAndClear());*/
            throw new ArgumentNullException("something went wrong !!!");
        }
        
        return await Task.FromResult(response!);
        
        //return (await query.GetQuery<ProductDto>())!;
    }
}