using HostedService.Application.Interfaces.Repositories;
using HostedService.Infrastructure.Settings;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Model.Extensions;

namespace HostedService.Infrastructure.Repositories;

public class CartRepository(IOptions<CartSettings> cartOptions, IDistributedCache cache) : ICartRepository
{
    private const string NsCacheKey = "NSCartIds"; // not synced cartIds key
    private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(initialCount: 3, maxCount: 3);
    private readonly string _baseUrl = cartOptions.Value.Address + "/api/cart-service";
    public async Task SaveCachedCartsToDbAsync()
    {
        var cartIds = await cache.GetRecordAsync<List<Guid>>(NsCacheKey);
        if (cartIds is null || cartIds.Count == 0) return;
        var saveTasks = cartIds.Select(async cartId =>
        {
            await Semaphore.WaitAsync();
            try
            {
                await $"{_baseUrl}/cart/cache-to-db?cartId={cartId}".PostQuery();
            }
            finally
            {
                Semaphore.Release();
            }
        });
        await Task.WhenAll(saveTasks);
    }
}