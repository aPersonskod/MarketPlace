using Cart.Application.Interfaces.Repositories;
using Cart.Infrastructure.Data;
using Model;

namespace Cart.Infrastructure.Repositories;

public class PlaceRepository(AppDbContext context) : IPlaceRepository
{
    public Task<IEnumerable<Place>> GetAllPlacesAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Place> GetPlaceByIdAsync(Guid placeId)
    {
        throw new NotImplementedException();
    }
}