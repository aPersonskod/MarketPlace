using Microsoft.Extensions.Options;
using Models.Dtos;
using Models.Extensions;
using ShoppingCartsWorkerService.Settings;

namespace ShoppingCartsWorkerService;

public interface IBuyCartService
{
    Task BuyCartAsync(CartDto cartDto, string accessToken);
}

public class BuyCartService(IOptions<BuyActionsSettings> options) : IBuyCartService
{
    private readonly string _baseAddress = options.Value.Address;
    public async Task BuyCartAsync(CartDto cartDto, string accessToken) 
        => await $"{_baseAddress}/BuyCart".PostQuery(cartDto, accessToken);
}