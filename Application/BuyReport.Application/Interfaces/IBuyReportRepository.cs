namespace BuyReport.Application.Interfaces;

public interface IBuyReportRepository
{
    Task<Model.BuyReport?> GetReportByCartIdAsync(Guid cartId);
    Task<Model.BuyReport> CreateBuyReportByCartIdAsync(Guid cartId);
}