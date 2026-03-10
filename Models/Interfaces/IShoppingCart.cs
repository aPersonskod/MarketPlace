using Models.Dtos;

namespace Models.Interfaces;

public interface IShoppingCart
{
    Task<IEnumerable<CartDto>> GetCarts();
    Task<IEnumerable<PlaceDto>> GetPlaces();
    Task<PlaceDto> GetPlace(Guid placeId);
    Task<IEnumerable<OrderDto>> GetOrders(Guid cartId);
    Task<CartDto> GetCart(string? accessToken);
    Task<CartDto> GetCartById(Guid cartId);
    Task<CartDto> AddOrder(Guid productId, int quantity, string? accessToken = null);
    Task<CartDto> ConfirmCart(Guid placeId, string? accessToken = null);
    Task<CartDto> ConfirmAndBuyCart(Guid? placeId, string? accessToken = null);
    Task MarkCartAsBought(Guid cartId);
    Task<CartDto> DeleteOrder(Guid productId, string? accessToken = null);
}