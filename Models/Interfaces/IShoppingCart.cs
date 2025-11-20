namespace Models;

public interface IShoppingCart
{
    Task<Cart> Get();
    Task<Cart> AddOrder(Guid productId, int quantity);
    Task<Cart> DeleteOrder(Guid productId);
}