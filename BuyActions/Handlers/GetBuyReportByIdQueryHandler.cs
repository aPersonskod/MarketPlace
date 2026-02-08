using BuyActions.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Dtos;

namespace BuyActions.Handlers;

public class GetBuyReportByIdQueryHandler(DataContext dataContext, IMediator mediator)
    : IRequestHandler<GetBuyReportByIdQuery, BuyReportDto?>
{
    public async Task<BuyReportDto?> Handle(GetBuyReportByIdQuery request, CancellationToken cancellationToken)
    {
        var buyReport =
            await dataContext.BuyReports.FirstOrDefaultAsync(x => x.Id == request.ReportId, cancellationToken);
        if (buyReport == null) throw new Exception("Buy report not found");
        return await mediator.Send(new GetBuyReportQuery(buyReport), cancellationToken);
    }
}