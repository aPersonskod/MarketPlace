using Cart.Application.Dtos;

namespace Cart.Application.Interfaces.Services;

public interface IBuyReportService
{
    Task<IEnumerable<CartForReportDetailedDto>> GetCartsForReportAsync(Guid userId);
}