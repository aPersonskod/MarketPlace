using Microsoft.Extensions.Options;
using Model.Extensions;
using Orchestrator.Application.Dtos;
using Orchestrator.Application.Interfaces;
using Orchestrator.Infrastructure.Settings;

namespace Orchestrator.Infrastructure.Repositories;

public class CartRepository(IOptions<CartSettings> cartOptions) : ICartRepository
{
    private readonly string _baseUrl = cartOptions.Value.Address + "/api/cart-service";
    public async Task<CartDto?> ConfirmCartAsync(string authToken, Guid placeId)
    {
        var url = $"{_baseUrl}/confirm-cart?placeId={placeId}";
        return await url.PatchQuery<CartDto>(authToken);
    }

    public async Task<CartDto?> UnConfirmCartAsync(string authToken)
    {
        var url = $"{_baseUrl}/unconfirm-cart";
        return await url.PatchQuery<CartDto>(authToken);
    }

    public async Task<CartDto?> BuyCartAsync(string authToken, Guid cartId)
    {
        var url = $"{_baseUrl}/mark-cart-as-bought/{cartId}";
        return await url.PatchQuery<CartDto>(authToken);
    }

    public async Task<CartDto?> BuyBackCartAsync(string authToken, Guid cartId)
    {
        var url = $"{_baseUrl}/mark-cart-as-not-bought/{cartId}";
        return await url.PatchQuery<CartDto>(authToken);
    }

    public async Task CachedCartDataToDbAsync(Guid cartId)
    {
        var urlOrders = $"{_baseUrl}/orders/cache-to-db?cartId={cartId}";
        var urlCart = $"{_baseUrl}/cart/cache-to-db?cartId={cartId}";
        await urlOrders.PostQuery();
        await urlCart.PostQuery();
    }
}

public class TestCartRepository : ICartRepository
{
    private readonly CartDto _cartDto;

    public TestCartRepository()
    {
        _cartDto = new CartDto()
        {
            Id = Guid.Parse("b8a0ff81-f0f6-405a-bfc5-ebb0e09fac8b"),
            PlaceId = null,
            UserId = Guid.NewGuid(),
            AmountToPay = 10,
            IsConfirmed = false,
            IsBought = false
        };
    }
    public Task<CartDto?> ConfirmCartAsync(string authToken, Guid placeId)
    {
        _cartDto.PlaceId = placeId;
        _cartDto.IsConfirmed = true;
        return Task.FromResult(_cartDto)!;
    }

    public Task<CartDto?> UnConfirmCartAsync(string authToken)
    {
        _cartDto.PlaceId = null;
        _cartDto.IsConfirmed = false;
        return Task.FromResult(_cartDto)!;
    }

    public Task<CartDto?> BuyCartAsync(string authToken, Guid cartId)
    {
        _cartDto.IsBought = true;
        return Task.FromResult(_cartDto)!;
    }

    public Task<CartDto?> BuyBackCartAsync(string authToken, Guid cartId)
    {
        _cartDto.IsBought = false;
        return Task.FromResult(_cartDto)!;
    }

    public Task CachedCartDataToDbAsync(Guid cartId)
    {
        throw new NotImplementedException();
    }
}