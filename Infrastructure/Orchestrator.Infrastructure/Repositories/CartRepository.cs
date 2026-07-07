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
}