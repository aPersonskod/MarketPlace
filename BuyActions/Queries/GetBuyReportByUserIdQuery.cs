using MediatR;
using Models.Dtos;

namespace BuyActions.Queries;

public record GetBuyReportByUserIdQuery(string AccessToken) : IRequest<IEnumerable<BuyReportDto?>>;