using Cart.Application.Dtos;

namespace Cart.Application.Interfaces.Services;

public interface IPlaceService
{
    Task<IEnumerable<PlaceDto>> GetAllPlacesAsync();
    Task<PlaceDto> GetPlaceAsync(Guid placeId);
}