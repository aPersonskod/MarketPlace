using MediatR;
using Models.Dtos;

namespace BuyActions.Queries;

public record GetCartPlaceQuery(Guid PlaceId) : IRequest<PlaceDto?>;