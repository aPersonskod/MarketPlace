using Cart.Application.Interfaces;
using Cart.Application.Interfaces.Repositories.Cached;
using Cart.Infrastructure.CacheModels;
using Microsoft.Extensions.Caching.Distributed;
using Model.SharedExceptions;

namespace Cart.Infrastructure.Repositories.Cached;

public class CachedCartRepository(IUnitOfWork unitOfWork, IDistributedCache cache) : ICachedCartRepository
{
    private async Task<Guid> GetNotBoughtCartIdAsync(Guid userId)
    {
        var cartId = await cache.GetCartIdByUserId(userId);
        if (cartId != Guid.Empty) return cartId;
        var cart = await unitOfWork.CartRepository.GetNotBoughtCartByUserIdAsync(userId);
        if (cart == null) return Guid.Empty;
        await cache.SetCartIdByUserId(userId, cart.Id);
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
        var cachedCart = await cache.GetCartByCartId(cartId);
        if (cachedCart == null)
        {
            cachedCart = await unitOfWork.CartRepository.GetCartByIdAsync(cartId);
            if (cachedCart == null) return null;
            await cache.SetCartByCartId(cartId, cachedCart);
        }
        if (cachedCart == null) throw new NotFoundException("Cached cart not found, cache is not working");
        return cachedCart;
    }
    public async Task<Model.Cart> AddCartAsync(Model.Cart cart)
    {
        await cache.SetCartByCartId(cart.Id, cart);
        await cache.SetCartIdByUserId(cart.UserId, cart.Id);
        return await cache.GetCartByCartId(cart.Id) 
               ?? throw new NotFoundException("Cached cart not found, cache is not working");
    }
    
    public async Task UpdateAmountToPayAsync(Guid cartId, IEnumerable<(int productCost, int productQuantity)>? costCollection)
    {
        var cachedCart = await GetCartByIdAsync(cartId);
        if (cachedCart == null) throw new NotFoundException("Cart not found");
        if (costCollection == null) throw new NotFoundException("Cart has no orders");
        cachedCart.AmountToPay = cachedCart.UpdateAmountToPay(costCollection);
        await cache.SetCartByCartId(cartId, cachedCart);
    }
    public async Task DeleteCartAsync(Guid cartId)
    {
        var foundItem = await GetCartByIdAsync(cartId);
        if (foundItem == null) throw new NotFoundException("Cart not found");
        // remove cached cart
        await cache.RemoveAsync(CartCache.CartKey(foundItem.Id));
        // remove cart from ns list
        await cache.DeleteChangedCartId(foundItem.Id);
    }
}