using Cart.Application.Interfaces;
using Cart.Application.Interfaces.Repositories.Cached;
using Microsoft.Extensions.Caching.Distributed;
using Model.Extensions;
using Model.SharedExceptions;

namespace Cart.Infrastructure.Repositories.Cached;

public class CachedCartRepository(IUnitOfWork unitOfWork, IDistributedCache cache) : ICachedCartRepository
{
    private static string CacheKey (Guid cartId) => $"cart:{cartId}";
    private static string CacheUserKey (Guid userId) => $"user:{userId}";

    private async Task<Guid> GetNotBoughtCartIdAsync(Guid userId)
    {
        var cartId = await cache.GetRecordAsync<Guid>(CacheUserKey(userId));
        if (cartId != Guid.Empty) return cartId;
        var cart = await unitOfWork.CartRepository.GetNotBoughtCartByUserIdAsync(userId);
        if (cart == null) return Guid.Empty;
        await cache.SetRecordAsync(CacheUserKey(userId), cart.Id);
        return cart.Id;
    }
    public async Task<Model.Cart?> GetNotBoughtCartByUserIdAsync(Guid userId)
    {
        var cartId = await GetNotBoughtCartIdAsync(userId);
        if (cartId == Guid.Empty) return null;
        return await GetCartByIdAsync(cartId);
    }

    public async Task<Model.Cart?> GetCartByIdAsync(Guid cartId)
    {
        var cachedCart = await cache.GetRecordAsync<Model.Cart>(CacheKey(cartId));
        if (cachedCart == null)
        {
            cachedCart = await unitOfWork.CartRepository.GetCartByIdAsync(cartId);
            if (cachedCart == null) return null;
            await cache.SetRecordAsync(CacheKey(cartId), cachedCart);
        }
        if (cachedCart == null) throw new NotFoundException("Cached cart not found, cache is not working");
        return cachedCart;
    }
    public async Task<Model.Cart> AddCartAsync(Guid userId)
    {
        var createdInDbCart = await GetNotBoughtCartByUserIdAsync(userId);
        if (createdInDbCart == null) throw new NotFoundException("Failed to save cart to database");
        await cache.SetRecordAsync(CacheKey(createdInDbCart.Id), createdInDbCart);
        return createdInDbCart;
    }
    
    public async Task UpdateAmountToPayAsync(Guid cartId, IEnumerable<(int productCost, int productQuantity)>? costCollection)
    {
        var cachedCart = await GetCartByIdAsync(cartId);
        if (cachedCart == null) throw new NotFoundException("Cart not found");
        if (costCollection == null) throw new NotFoundException("Cart has no orders");
        cachedCart.AmountToPay = cachedCart.UpdateAmountToPay(costCollection);
        await cache.SetRecordAsync(CacheKey(cartId), cachedCart);
    }
    public async Task DeleteCartAsync(Guid cartId)
    {
        var foundItem = await GetCartByIdAsync(cartId);
        if (foundItem == null) throw new NotFoundException("Cart not found");
        await cache.RemoveAsync(CacheKey(foundItem.Id));
    }
}