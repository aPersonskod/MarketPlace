using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Model.SharedExceptions;

namespace Model.Extensions;

public static class DistributedCacheExt
{
    public static async Task SetRecordAsync<T>(this IDistributedCache cache, string recordId, T data, 
        bool isSynced = false, TimeSpan expiry = default)
    {
        var cachedObject = new CachedObject<T>(data, isSynced);
        var jsonData = JsonSerializer.Serialize(cachedObject);
        if (expiry == default) expiry = TimeSpan.FromMinutes(10);
        await cache.SetStringAsync(recordId, jsonData, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = expiry });
    }

    public static async Task<CachedObject<T>?> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
    {
        var jsonData = await cache.GetStringAsync(recordId);
        return jsonData == null ? default! : JsonSerializer.Deserialize<CachedObject<T>>(jsonData)!;
    }
}