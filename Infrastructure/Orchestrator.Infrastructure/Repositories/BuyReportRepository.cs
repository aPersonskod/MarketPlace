using Microsoft.Extensions.Options;
using Model.Extensions;
using Orchestrator.Application.Dtos;
using Orchestrator.Application.Interfaces;
using Orchestrator.Infrastructure.Settings;

namespace Orchestrator.Infrastructure.Repositories;

public class BuyReportRepository(IOptions<BuyReportSettings> buyOptions) : IBuyReportRepository
{
    private readonly string _baseUrl = buyOptions.Value.Address + "/api/buy-service";
    public async Task<BuyReportDto?> CreateBuyReportAsync(CreateBuyReportDto createBuyReportDto)
    {
        var url = $"{_baseUrl}/create-report";
        var body = new CreateBuyReportBodyDto(createBuyReportDto.CartId);
        return await url.PostQuery<BuyReportDto, CreateBuyReportBodyDto>(body, createBuyReportDto.AuthToken);
    }
}