using MediatR;
using Models.Dtos;

namespace BuyActions.Queries;

public record GetCartOrdersQuery(Guid CartId) : IRequest<IEnumerable<OrderDto>?>;