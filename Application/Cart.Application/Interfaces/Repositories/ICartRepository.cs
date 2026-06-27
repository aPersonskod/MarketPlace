namespace Cart.Application.Interfaces.Repositories;

public interface ICartRepository
{
    Task<IEnumerable<Model.Cart>> GetBoughtCartsAsync(Guid userId);
    Task<Model.Cart?> GetUnverifiedCartByUserIdAsync(Guid userId);
    Task<Model.Cart?> GetCartByIdAsync(Guid cartId);
    Task<Model.Cart> AddCartAsync(Guid userId);
    Task<Model.Cart> AddPlaceToCart(Guid userId, Guid placeId);
    Task<Model.Cart> ConfirmCartAsync(Guid userId);
    Task<Model.Cart> UnConfirmCartAsync(Guid userId);
    Task<Model.Cart> BuyCartAsync(Guid cartId);
    Task<Model.Cart> BuyBackCartAsync(Guid cartId);
    Task UpdateAmountToPayAsync(Guid cartId, IEnumerable<(int productCost, int productQuantity)> costCollection);
    Task DeleteCartAsync(Guid cartId);
}