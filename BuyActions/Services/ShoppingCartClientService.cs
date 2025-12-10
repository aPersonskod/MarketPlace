using BuyActions.Settings;
using Microsoft.Extensions.Options;
using Models;
using Models.Dtos;
using Models.Extensions;

namespace BuyActions.Services;

public class ShoppingCartClientService(IOptions<ShoppingCartSettings> shoppingCartOptions)
{
    private readonly string _baseAddress = $"{shoppingCartOptions.Value.Address}";

    public async Task MarkCartAsBought(Guid cartId)
    {
        var query = $"{_baseAddress}/MarkCartAsBought?cartId={cartId}";
        await query.PostQuery();
    }

    public async Task<IEnumerable<OrderDto>?> GetCartOrders(Guid cartId)
    {
        var query = $"{_baseAddress}/GetCartOrders?cartId={cartId}";
        return await query.GetQuery<IEnumerable<OrderDto>>();
    }

    public async Task<PlaceDto?> GetPlace(Guid? placeId)
    {
        var query = $"{_baseAddress}/GetPlace?placeId={placeId}";
        return await query.GetQuery<PlaceDto>();
    }

    public async Task<CartDto?> GetCartById(Guid cartId)
    {
        var query = $"{_baseAddress}/GetCartById?cartId={cartId}";
        return await query.GetQuery<CartDto>();
    }
}