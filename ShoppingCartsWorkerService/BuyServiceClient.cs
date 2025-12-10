using Microsoft.Extensions.Options;
using Models.Dtos;
using Models.Extensions;
using Models.Interfaces;
using ShoppingCartsWorkerService.Settings;

namespace ShoppingCartsWorkerService;

public class BuyServiceClient(IOptions<BuyActionsSettings> options) : IBuyService
{
    private readonly string _baseAddress = options.Value.Address;
    public async Task<IEnumerable<BuyReportDto>> Get() => (await $"{_baseAddress}".GetQuery<IEnumerable<BuyReportDto>>())!;
    public async Task<IEnumerable<BuyReportDto>> GetByUserId(Guid userId) 
        => (await $"{_baseAddress}/GetByUserId?userId={userId}".GetQuery<IEnumerable<BuyReportDto>>())!;
    public async Task<BuyReportDto> Get(Guid reportId) => (await $"{_baseAddress}/{reportId}".GetQuery<BuyReportDto>())!;
    public async Task BuyCart(CartDto cartDto) => await $"{_baseAddress}/BuyCart".PostQuery(cartDto);
}