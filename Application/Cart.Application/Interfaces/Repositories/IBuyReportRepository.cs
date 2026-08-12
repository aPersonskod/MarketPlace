using Cart.Application.Dtos;

namespace Cart.Application.Interfaces.Repositories;

public interface IBuyReportRepository
{
    Task<IEnumerable<CartForReportDto>?> GetCartsForReportAsync(Guid userId);
}