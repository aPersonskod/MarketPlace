using Orchestrator.Application.Dtos;
using Orchestrator.Application.Interfaces;

namespace Orchestrator.Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    public Task<CartDto> ConfirmCartAsync(string authToken, Guid placeId)
    {
        throw new NotImplementedException();
    }

    public Task<CartDto> UnConfirmCartAsync(string authToken)
    {
        throw new NotImplementedException();
    }

    public Task<CartDto> BuyCartAsync(string authToken, Guid cartId)
    {
        throw new NotImplementedException();
    }

    public Task<CartDto> BuyBackCartAsync(string authToken, Guid cartId)
    {
        throw new NotImplementedException();
    }
}