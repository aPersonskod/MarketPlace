using Model;

namespace Cart.Application.Interfaces.Repositories;

public interface IPlaceRepository
{
    Task<IEnumerable<Place>> GetAllPlacesAsync();
    Task<Place> GetPlaceByIdAsync(Guid placeId);
}