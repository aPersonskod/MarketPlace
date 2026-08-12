using BuyReport.Application.Dtos;

namespace BuyReport.Application.Interfaces;

public interface ICartRepository
{
    [Obsolete]
    Task<IEnumerable<CartForReportDto>?> GetCartsForReportAsync(string? authToken);
    Task<CartForReportDto?> GetCartForReportAsync(Guid cartId, string? authToken);
    Task<bool> IsCartExistsAsync(Guid cartId, string? authToken);
}