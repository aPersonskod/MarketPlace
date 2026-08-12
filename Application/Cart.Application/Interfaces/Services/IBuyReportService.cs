using Cart.Application.Dtos;

namespace Cart.Application.Interfaces.Services;

public interface IBuyReportService
{
    [Obsolete]
    Task<IEnumerable<CartForReportDetailedDto>> GetCartsForReportAsync(Guid userId);
    Task<CartForReportDetailedDto?> GetCartForReportAsync(Guid cartId);
}