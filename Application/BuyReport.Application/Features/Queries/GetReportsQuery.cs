using BuyReport.Application.Dtos;
using BuyReport.Application.Interfaces;
using BuyReport.Application.Mappings;
using Model.SharedExceptions;
using MediatR;

namespace BuyReport.Application.Features.Queries;

public record GetReportsQuery(string? AuthToken) : IRequest<IEnumerable<DetailedBuyReportDto>>;

public class GetReportsQueryHandler(IBuyReportRepository buyReportRepository, ICartRepository cartRepository, 
    IUserRepository userRepository) : IRequestHandler<GetReportsQuery, IEnumerable<DetailedBuyReportDto>>
{
    private readonly List<DetailedBuyReportDto> _detailedBuyReportDtos = [];
    public async Task<IEnumerable<DetailedBuyReportDto>> Handle(GetReportsQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var cartReports = await cartRepository.GetCartsForReportAsync(request.AuthToken);
        if (cartReports is null) throw new NotFoundException("Cart reports not found");
        var userDto = await userRepository.GetUserAsync(request.AuthToken);
        foreach (var cartForReportDto in cartReports)
        {
            var buyReport = await buyReportRepository.GetReportByCartIdAsync(cartForReportDto.CartId);
            if (buyReport == null) continue;
            var detailedReport = new DetailedBuyReportDto()
            {
                DetailedCartReportDto = cartForReportDto.ToDetailedDto(userDto),
                SaleDate = buyReport.SaleDate
            };
            _detailedBuyReportDtos.Add(detailedReport);
        }
        return _detailedBuyReportDtos;
    }
}