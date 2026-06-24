using BuyReport.Application.Dtos;

namespace BuyReport.Application.Interfaces;

public interface ICartRepository
{
    Task<IEnumerable<CartForReportDto>?> GetCartsForReportAsync(string? authToken);
    Task<bool> IsCartExistsAsync(Guid cartId, string? authToken);
}