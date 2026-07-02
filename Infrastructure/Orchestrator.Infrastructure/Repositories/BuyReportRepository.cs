using Orchestrator.Application.Dtos;
using Orchestrator.Application.Interfaces;

namespace Orchestrator.Infrastructure.Repositories;

public class BuyReportRepository : IBuyReportRepository
{
    public Task<BuyReportDto?> CreateBuyReportByCartIdAsync(CreateBuyReportDto createBuyReportDto)
    {
        throw new NotImplementedException();
    }
}