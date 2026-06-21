namespace Cart.Application.Interfaces.Repositories;

public interface ICartRepository
{
    Task<IEnumerable<Model.Cart>> GetBoughtCartsAsync(Guid userId);
    Task<Model.Cart?> GetCartByUserIdAsync(Guid userId);
    Task<Model.Cart> AddCartAsync(Guid userId);
    Task<Model.Cart> ConfirmCartAsync(Guid cartId);
    Task<Model.Cart> UnConfirmCartAsync(Guid cartId);
    Task<Model.Cart> BuyCartAsync(Guid cartId);
    Task<Model.Cart> BuyBackCartAsync(Guid cartId);
    Task UpdateAmountToPayAsync(Guid cartId, int productCost);
    Task DeleteCartAsync(Guid cartId);
}