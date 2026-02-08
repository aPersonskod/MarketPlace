using MediatR;
using Models.Dtos;

namespace BuyActions.Queries;

public record GetBuyReportsQuery() : IRequest<IAsyncEnumerable<BuyReportDto?>>;