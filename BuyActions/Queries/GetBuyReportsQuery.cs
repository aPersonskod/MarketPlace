using MediatR;
using Models.Dtos;

namespace BuyActions.Queries;

public record GetBuyReportsQuery(string AccessToken) : IRequest<IAsyncEnumerable<BuyReportDto?>>;