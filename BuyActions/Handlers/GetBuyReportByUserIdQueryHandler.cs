using BuyActions.Queries;
using MediatR;
using Models.Dtos;

namespace BuyActions.Handlers;

public class GetBuyReportByUserIdQueryHandler(DataContext dataContext, IMediator mediator)
    : IRequestHandler<GetBuyReportByUserIdQuery, IEnumerable<BuyReportDto?>>
{
    public async Task<IEnumerable<BuyReportDto?>> Handle(GetBuyReportByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var buyReportDtos = new List<BuyReportDto>();
        await foreach (var buyReport in dataContext.BuyReports)
        {
            var buyReportDto = await mediator.Send(new GetBuyReportQuery(buyReport), cancellationToken);
            if (buyReportDto?.BuyReportCart.User.Id == request.UserId) buyReportDtos.Add(buyReportDto);
        }

        return await Task.FromResult<IEnumerable<BuyReportDto>>(buyReportDtos);
    }
}