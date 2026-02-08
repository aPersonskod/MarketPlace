using MediatR;
using Models.Dtos;

namespace BuyActions.Queries;

public record GetBuyReportByUserIdQuery(Guid UserId) : IRequest<IEnumerable<BuyReportDto?>>;