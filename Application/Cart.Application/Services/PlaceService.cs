using Cart.Application.Dtos;
using Cart.Application.Interfaces;
using Cart.Application.Interfaces.Services;
using Cart.Application.Mappings;

namespace Cart.Application.Services;

public class PlaceService(IUnitOfWork unitOfWork) : IPlaceService
{
    public async Task<IEnumerable<PlaceDto>> GetAllPlacesAsync()
    {
        var places = await unitOfWork.PlaceRepository.GetAllPlacesAsync();
        return places.Select(x => x.ToDto());
    }
    public async Task<PlaceDto> GetPlaceAsync(Guid placeId)
    {
        var place = await unitOfWork.PlaceRepository.GetPlaceByIdAsync(placeId);
        return place.ToDto();
    }
}