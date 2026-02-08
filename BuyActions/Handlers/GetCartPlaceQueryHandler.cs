using BuyActions.Queries;
using BuyActions.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using Models.Dtos;
using Models.Extensions;

namespace BuyActions.Handlers;

public class GetCartPlaceQueryHandler(IOptions<ShoppingCartSettings> cartOptions)
    : IRequestHandler<GetCartPlaceQuery, PlaceDto?>
{
    public async Task<PlaceDto?> Handle(GetCartPlaceQuery request, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return await Task.FromResult<PlaceDto?>(null);
        
        var query = $"{cartOptions.Value.Address}/GetPlace?placeId={request.PlaceId}";
        return await query.GetQuery<PlaceDto>();
    }
}