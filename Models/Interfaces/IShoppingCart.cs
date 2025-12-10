using Models.Dtos;

namespace Models.Interfaces;

public interface IShoppingCart
{
    Task<IEnumerable<CartDto>> GetCarts();
    Task<IEnumerable<PlaceDto>> GetPlaces();
    Task<PlaceDto> GetPlace(Guid placeId);
    Task<IEnumerable<OrderDto>> GetOrders(Guid cartId);
    Task<CartDto> GetCart(Guid userId);
    Task<CartDto> GetCartById(Guid cartId);
    Task<CartDto> AddOrder(Guid userId, Guid productId, int quantity);
    Task<CartDto> ConfirmCart(Guid userId, Guid placeId);
    Task<CartDto> ConfirmAndBuyCart(Guid userId, Guid placeId);
    Task MarkCartAsBought(Guid cartId);
    Task<CartDto> DeleteOrder(Guid userId, Guid productId);
}