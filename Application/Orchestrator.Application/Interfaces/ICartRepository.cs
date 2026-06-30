using Orchestrator.Application.Dtos;

namespace Orchestrator.Application.Interfaces;

public interface ICartRepository
{
    Task<CartDto> ConfirmCartAsync(string authToken, Guid placeId);
    Task<CartDto> UnConfirmCartAsync(string authToken);
    
    Task<CartDto> BuyCartAsync(string authToken, Guid cartId);
    Task<CartDto> BuyBackCartAsync(string authToken, Guid cartId);
}