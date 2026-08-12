using BuyReport.Application.Dtos;
using BuyReport.Application.Interfaces;
using BuyReport.Application.Mappings;
using Model.SharedExceptions;
using MediatR;

namespace BuyReport.Application.Features.Queries;

public record GetReportsQuery(Guid UserId, string? AuthToken, int PageNumber, int PageSize) : IRequest<PaginatedDetailedBuyReportsDto>;

public class GetReportsQueryHandler(IBuyReportRepository buyReportRepository, ICartRepository cartRepository, 
    IUserRepository userRepository) : IRequestHandler<GetReportsQuery, PaginatedDetailedBuyReportsDto>
{
    private readonly List<DetailedBuyReportDto> _detailedBuyReportDtos = [];
    public async Task<PaginatedDetailedBuyReportsDto> Handle(GetReportsQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var detailedBuyReports = new List<DetailedBuyReportDto>();
        var userDto = await userRepository.GetUserAsync(request.AuthToken);
        var paginatedReports = await buyReportRepository.GetReportsByUserIdAsync(request.UserId, request.PageNumber, request.PageSize);
        if (paginatedReports is null) return new PaginatedDetailedBuyReportsDto()
        {
            CurrentPage = request.PageNumber,
            PageSize = request.PageSize,
            RecordsCount = 0,
            Reports = detailedBuyReports
        };
        foreach (var paginatedReport in paginatedReports)
        {
            var detailedCartReport = await cartRepository.GetCartForReportAsync(paginatedReport.CartId, request.AuthToken);
            if (detailedCartReport is null) continue;
            detailedBuyReports.Add(new DetailedBuyReportDto()
            {
                DetailedCartReportDto = detailedCartReport.ToDetailedDto(userDto),
                SaleDate = paginatedReport.SaleDate    
            });
        }
        return new PaginatedDetailedBuyReportsDto()
        {
            CurrentPage = request.PageNumber,
            PageSize = request.PageSize,
            RecordsCount = detailedBuyReports.Count,
            Reports = detailedBuyReports
        };
        
        
        /*var cartReports = await cartRepository.GetCartsForReportAsync(request.AuthToken);
        if (cartReports is null) throw new NotFoundException("Cart reports not found");
        var carts = cartReports.ToList();
        var userDto = await userRepository.GetUserAsync(request.AuthToken);
        foreach (var cartForReportDto in carts)
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

        var reports = _detailedBuyReportDtos.OrderByDescending(x => x.SaleDate).Skip(skipAmount).Take(pageSize).ToList();
        return new PaginatedDetailedBuyReportsDto()
        {
            RecordsCount = _detailedBuyReportDtos.Count,
            CurrentPage = pageNumber,
            PageSize = pageSize,
            Reports = reports
        };*/
    }
}