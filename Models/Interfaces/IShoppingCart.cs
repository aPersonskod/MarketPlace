namespace Models;

public interface IShoppingCart
{
    Task<IEnumerable<Cart>> Get();
    Task<IEnumerable<Place>> GetPlaces();
    Task<Cart> Get(Guid userId);
    Task<Cart> AddOrder(Guid userId, Guid productId, Guid placeId, int quantity);
    Task DeleteOrder(Guid userId, Guid productId);
}