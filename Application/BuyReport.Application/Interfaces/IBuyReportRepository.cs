namespace BuyReport.Application.Interfaces;

public interface IBuyReportRepository
{
    Task<Model.BuyReport?> GetReportByCartIdAsync(Guid cartId);
    Task<IEnumerable<Model.BuyReport>?> GetReportsByUserIdAsync(Guid userId, int pageNumber, int pageSize);
    Task<Model.BuyReport> CreateBuyReportByCartIdAsync(Guid cartId, Guid userId);
}