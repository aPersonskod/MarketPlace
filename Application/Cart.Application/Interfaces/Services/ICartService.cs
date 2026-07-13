using Cart.Application.Dtos;

namespace Cart.Application.Interfaces.Services;

public interface ICartService
{
    Task<IEnumerable<CartDto>> GetBoughtCartsAsync(Guid userId);
    Task<CartDto> GetCartByUserIdAsync(Guid userId);
    Task<bool> GetCartByIdAsync(Guid cartId);
    Task DeleteCartAsync(Guid cartId);
    
    // saga
    Task<CartDto> ConfirmCartAsync(Guid userId, Guid placeId);
    Task<CartDto> UnConfirmCartAsync(Guid userId);
    
    Task<CartDto> MarkCartAsBoughtAsync(Guid cartId);
    Task<CartDto> MarkCartAsNotBoughtAsync(Guid cartId);
}