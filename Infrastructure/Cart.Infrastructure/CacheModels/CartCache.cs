using Microsoft.Extensions.Caching.Distributed;
using Model.Extensions;

namespace Cart.Infrastructure.CacheModels;

public static class CartCache
{
    internal const string ChangedCartIdsKey = "ChangedCartIds"; // not synced cartIds key
    internal static string CartKey (Guid cartId) => $"cart:{cartId}";
    
    public static async Task<Guid> GetCartIdByUserId (this IDistributedCache cache, Guid userId) 
        => await cache.GetRecordAsync<Guid>(UserCache.UserKey(userId));
    public static async Task SetCartIdByUserId (this IDistributedCache cache, Guid userId, Guid cartId) 
        => await cache.SetRecordAsync(UserCache.UserKey(userId), cartId);
    
    public static async Task<Model.Cart?> GetCartByCartId (this IDistributedCache cache, Guid cartId) 
        => await cache.GetRecordAsync<Model.Cart>(CartKey(cartId));
    public static async Task SetCartByCartId (this IDistributedCache cache, Guid cartId, Model.Cart cart) 
        => await cache.SetRecordAsync(CartKey(cartId), cart);
    
    public static async Task<IEnumerable<Guid>?> GetChangedCartIds (this IDistributedCache cache) 
        => await cache.GetRecordAsync<IEnumerable<Guid>>(ChangedCartIdsKey);
    public static async Task AddChangedCartId(this IDistributedCache cache, Guid cartId)
    {
        var nsCartIds = await cache.GetRecordAsync<IEnumerable<Guid>>(ChangedCartIdsKey);
        if (nsCartIds == null)
        {
            await cache.SetRecordAsync<IEnumerable<Guid>>(ChangedCartIdsKey, [cartId]);
        }
        else
        {
            var cartIds = nsCartIds.ToList();
            cartIds.Add(cartId);
            await cache.SetRecordAsync(ChangedCartIdsKey, cartIds.Distinct());
        }
    }
    public static async Task DeleteChangedCartId(this IDistributedCache cache, Guid cartId)
    {
        var nsCartIds = await cache.GetRecordAsync<IEnumerable<Guid>>(ChangedCartIdsKey);
        if (nsCartIds != null)
        {
            var cartIds = nsCartIds.ToList();
            cartIds.Remove(cartId);
            await cache.SetRecordAsync(ChangedCartIdsKey, cartIds.Distinct());
        }
    }
}

public class UserCache
{
    protected internal static string UserKey (Guid userId) => $"user:{userId}";
}

public static class OrderCache
{
    internal static string OrdersKey (Guid cartId) => $"orders:{cartId}";
    
    public static async Task<IEnumerable<Model.Order>?> GetCartOrdersByCartId (this IDistributedCache cache, Guid cartId)
        => await cache.GetRecordAsync<IEnumerable<Model.Order>>(OrdersKey(cartId));
    public static async Task SetCartOrdersByCartId(this IDistributedCache cache, Guid cartId, IEnumerable<Model.Order> orders) 
        => await cache.SetRecordAsync(OrdersKey(cartId), orders);
}