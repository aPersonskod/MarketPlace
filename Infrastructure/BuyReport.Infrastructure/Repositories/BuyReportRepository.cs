using BuyReport.Application.Interfaces;
using BuyReport.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BuyReport.Infrastructure.Repositories;

public class BuyReportRepository(AppDbContext context) : IBuyReportRepository
{
    public async Task<Model.BuyReport?> GetReportByCartIdAsync(Guid cartId)
        => await context.BuyReports.FirstOrDefaultAsync(x => x.CartId == cartId);

    public async Task<IEnumerable<Model.BuyReport>?> GetReportsByUserIdAsync(Guid userId, int pageNumber, int pageSize)
    {
        var skipAmount = (pageNumber - 1) * pageSize;
        return await context.BuyReports
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.SaleDate)
            .Skip(skipAmount)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Model.BuyReport> CreateBuyReportByCartIdAsync(Guid cartId, Guid userId)
    {
        var buyReport = Model.BuyReport.Create(userId, cartId, DateTime.Now);
        await context.BuyReports.AddAsync(buyReport);
        await context.SaveChangesAsync();
        return buyReport;
    }
}