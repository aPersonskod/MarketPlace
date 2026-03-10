using System.Runtime.CompilerServices;
using BuyActions.Queries;
using MediatR;
using Models.Dtos;

namespace BuyActions.Handlers;

public class GetBuyReportsQueryHandler(DataContext dataContext, IMediator mediator)
    : IRequestHandler<GetBuyReportsQuery, IAsyncEnumerable<BuyReportDto?>>
{
    public async Task<IAsyncEnumerable<BuyReportDto?>> Handle(GetBuyReportsQuery request,
        CancellationToken cancellationToken) => await Task.FromResult(Get(cancellationToken, request.AccessToken));

    private async IAsyncEnumerable<BuyReportDto?> Get([EnumeratorCancellation] CancellationToken cancellationToken,
        string accessToken)
    {
        await foreach (var buyReport in dataContext.BuyReports)
            yield return await mediator.Send(new GetBuyReportQuery(buyReport, accessToken), cancellationToken);
    }
}