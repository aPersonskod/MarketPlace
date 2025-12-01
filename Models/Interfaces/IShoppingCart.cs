namespace Models.Interfaces;

public interface IShoppingCart
{
    Task<IEnumerable<Cart>> Get();
    Task<IEnumerable<Place>> GetPlaces();
    Task<Cart> Get(Guid userId);
    Task<Cart> AddOrder(Guid userId, Guid productId, int quantity);
    Task<Cart> ConfirmCart(Guid userId, Guid placeId);
    Task MarkCartAsBought(Guid cartId);
    Task<Cart> DeleteOrder(Guid userId, Guid productId);
}