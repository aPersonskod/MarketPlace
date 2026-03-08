using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Models.Extensions;

public static class DisturbedCacheExt
{
    public static async Task SetRecordAsync<T>(this IDistributedCache cache, string recordId, T data, TimeSpan expiry = default)
    {
        var jsonData = JsonSerializer.Serialize(data);
        await cache.SetStringAsync(recordId, jsonData, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = expiry });
    }

    public static async Task<T?> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
    {
        var jsonData = await cache.GetStringAsync(recordId);
        return jsonData == null ? default! : JsonSerializer.Deserialize<T>(jsonData)!;
    }
}