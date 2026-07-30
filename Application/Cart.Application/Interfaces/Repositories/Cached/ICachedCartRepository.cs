namespace Cart.Application.Interfaces.Repositories.Cached;

public interface ICachedCartRepository
{
    Task<Model.Cart?> GetNotBoughtCartByUserIdAsync(Guid userId);
    Task<Model.Cart?> GetCartByIdAsync(Guid cartId);
    Task<Model.Cart> AddCartAsync(Guid userId);
    Task UpdateAmountToPayAsync(Guid cartId, IEnumerable<(int productCost, int productQuantity)>? costCollection);
    Task DeleteCartAsync(Guid cartId);
}