using BuyReport.Application.Interfaces;
using BuyReport.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BuyReport.Infrastructure.Repositories;

public class BuyReportRepository(AppDbContext context) : IBuyReportRepository
{
    public async Task<Model.BuyReport?> GetReportByCartIdAsync(Guid cartId) 
        => await context.BuyReports.FirstOrDefaultAsync(x => x.CartId == cartId);
    public async Task<Model.BuyReport> CreateBuyReportByIdAsync(Guid cartId)
    {
        var buyReport = Model.BuyReport.Create(cartId, DateTime.Now);
        await context.BuyReports.AddAsync(buyReport);
        return buyReport;
    }
}