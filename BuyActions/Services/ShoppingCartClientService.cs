using BuyActions.Settings;
using Microsoft.Extensions.Options;
using Models;

namespace BuyActions.Services;

public class ShoppingCartClientService(IOptions<ShoppingCartSettings> shoppingCartOptions)
{
    private readonly string _baseAddress = $"{shoppingCartOptions.Value.Address}";

    public async Task MarkCartAsBought(Guid cartId)
    {
        var query = $"{_baseAddress}/MarkCartAsBought?cartId={cartId}";
        await query.PostQuery();
    }
}