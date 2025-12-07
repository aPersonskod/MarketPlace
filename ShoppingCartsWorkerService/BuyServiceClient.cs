using Microsoft.Extensions.Options;
using Models;
using Models.Interfaces;
using ShoppingCartsWorkerService.Settings;

namespace ShoppingCartsWorkerService;

public class BuyServiceClient(IOptions<BuyActionsSettings> options) : IBuyService
{
    private readonly string _baseAddress = options.Value.Address;
    public async Task<IEnumerable<BuyReport>> Get() => (await $"{_baseAddress}".GetQuery<IEnumerable<BuyReport>>())!;

    public async Task<BuyReport> Get(Guid reportId) => (await $"{_baseAddress}/{reportId}".GetQuery<BuyReport>())!;

    public async Task BuyCart(Cart cart) => await $"{_baseAddress}/BuyCart".PostQuery(cart);
}