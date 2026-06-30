using Orchestrator.Application.Dtos;

namespace Orchestrator.Application.Interfaces;

public interface IBuyReportRepository
{
    Task<BuyReportDto?> CreateBuyReportByCartIdAsync(CreateBuyReportDto createBuyReportDto);
}