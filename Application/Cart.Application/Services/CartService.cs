using Cart.Application.Dtos;
using Cart.Application.Interfaces;
using Cart.Application.Interfaces.Services;
using Cart.Application.Mappings;
using Model.SharedExceptions;

namespace Cart.Application.Services;

public class CartService(IUnitOfWork unitOfWork) : ICartService
{
    public async Task<IEnumerable<CartDto>> GetBoughtCartsAsync(Guid userId)
    {
        var carts = await unitOfWork.CartRepository.GetBoughtCartsAsync(userId);
        return carts.Select(x => x.ToDto());
    }
    public async Task<CartDto> GetCartByUserIdAsync(Guid userId)
    {
        var cart = await unitOfWork.CartRepository.GetNotBoughtCartByUserIdAsync(userId);
        if (cart == null)
        {
            cart = await unitOfWork.CartRepository.AddCartAsync(userId);
            await unitOfWork.CompleteAsync();
        }
        return cart.ToDto();
    }
    public async Task<bool> GetCartByIdAsync(Guid cartId)
    {
        var cart = await unitOfWork.CartRepository.GetCartByIdAsync(cartId);
        return cart != null;
    }
    public async Task DeleteCartAsync(Guid cartId)
    {
        await unitOfWork.CartRepository.DeleteCartAsync(cartId);
        await unitOfWork.CompleteAsync();
    }
    public async Task<CartDto> ConfirmCartAsync(Guid userId, Guid placeId)
    {
        var place = await unitOfWork.PlaceRepository.GetPlaceByIdAsync(placeId);
        if (place == null) throw new NotFoundException("Place not found");
        await unitOfWork.CartRepository.AddPlaceToCart(userId, placeId);
        var cart = await unitOfWork.CartRepository.ConfirmCartAsync(userId);
        await unitOfWork.CompleteAsync();
        return cart.ToDto();
    }
    public async Task<CartDto> UnConfirmCartAsync(Guid userId)
    {
        var cart = await unitOfWork.CartRepository.UnConfirmCartAsync(userId);
        await unitOfWork.CompleteAsync();
        return cart.ToDto();
    }
    public async Task MarkCartAsBoughtAsync(Guid cartId)
    {
        await unitOfWork.CartRepository.BuyCartAsync(cartId);
        await unitOfWork.CompleteAsync();
    }
    public async Task MarkCartAsNotBoughtAsync(Guid cartId)
    {
        await unitOfWork.CartRepository.BuyBackCartAsync(cartId);
        await unitOfWork.CompleteAsync();
    }
}