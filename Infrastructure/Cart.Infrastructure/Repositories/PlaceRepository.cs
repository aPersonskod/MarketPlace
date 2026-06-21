using Cart.Application.Interfaces.Repositories;
using Cart.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Cart.Infrastructure.Repositories;

public class PlaceRepository(AppDbContext context) : IPlaceRepository
{
    public async Task<IEnumerable<Place>> GetAllPlacesAsync() 
        => await context.Places.ToListAsync();
    public async Task<Place?> GetPlaceByIdAsync(Guid placeId) 
        => await context.Places.FirstOrDefaultAsync(x => x.Id == placeId);
}