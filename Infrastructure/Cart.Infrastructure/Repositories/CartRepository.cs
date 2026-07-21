using Cart.Application.Dtos;
using Cart.Application.Interfaces;
using Cart.Application.Interfaces.Repositories;
using Cart.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Model.Extensions;
using Model.SharedExceptions;

namespace Cart.Infrastructure.Repositories;

public class CartRepository(AppDbContext context) : ICartRepository
{
    public async Task<IEnumerable<Model.Cart>> GetBoughtCartsAsync(Guid userId) 
        => await context.ShoppingCarts.Where(x => x.UserId == userId && x.IsBought).ToListAsync();
    public async Task<Model.Cart?> GetNotBoughtCartByUserIdAsync(Guid userId)
    {
        var carts = await context.ShoppingCarts.Where(x => x.UserId == userId).ToListAsync();
        return carts.FirstOrDefault(x => x.IsNotBought(x.UserId));
    }
    public async Task<Model.Cart?> GetCartByIdAsync(Guid cartId) 
        => await context.ShoppingCarts.FirstOrDefaultAsync(x => x.Id == cartId);
    public async Task<Model.Cart> AddCartAsync(Guid userId)
    {
        var cart = Model.Cart.CreateCart(userId);
        await context.ShoppingCarts.AddAsync(cart);
        return cart;
    }
    public async Task<Model.Cart> AddPlaceToCart(Guid userId, Guid placeId)
    {
        var cart = await GetNotBoughtCartByUserIdAsync(userId);
        if (cart == null) throw new NotFoundException("Cart not found");
        var foundCart = await context.ShoppingCarts.FirstAsync(x => x.Id == cart.Id);
        foundCart.PlaceId = placeId;
        return foundCart;
    }
    public async Task<Model.Cart> ConfirmCartAsync(Guid userId)
    {
        var cart = await GetNotBoughtCartByUserIdAsync(userId);
        if (cart == null) throw new NotFoundException("Cart not found");
        cart.ConfirmCart();
        return cart;
    }
    public async Task<Model.Cart> UnConfirmCartAsync(Guid userId)
    {
        var cart = await GetNotBoughtCartByUserIdAsync(userId);
        if (cart == null) throw new NotFoundException("Cart not found");
        cart.IsConfirmed = false;
        return cart;
    }
    public async Task<Model.Cart> BuyCartAsync(Guid cartId)
    {
        var cart = await GetCartByIdAsync(cartId);
        if (cart == null) throw new NotFoundException("Cart not found");
        cart.BuyCart();
        return cart;
    }
    public async Task<Model.Cart> BuyBackCartAsync(Guid cartId)
    {
        var cart = await GetCartByIdAsync(cartId);
        if (cart == null) throw new NotFoundException("Cart not found");
        cart.IsBought = false;
        return cart;
    }
    public async Task UpdateAmountToPayAsync(Guid cartId, IEnumerable<(int productCost, int productQuantity)>? costCollection)
    {
        var cart = await GetCartByIdAsync(cartId);
        if (cart == null) throw new NotFoundException("Cart not found");
        if (costCollection == null) throw new NotFoundException("Cart has no orders");
        cart.AmountToPay = cart.UpdateAmountToPay(costCollection);
    }
    public async Task DeleteCartAsync(Guid cartId)
    {
        var foundItem = await GetCartByIdAsync(cartId);
        if (foundItem == null) throw new NotFoundException("Cart not found");
        context.ShoppingCarts.Remove(foundItem);
    }
    public async Task UpdateCartAsync(Model.Cart cart)
    {
        var foundCart = await GetCartByIdAsync(cart.Id);
        if (foundCart == null) throw new NotFoundException("Cart not found");
        foundCart.AmountToPay = cart.AmountToPay;
        await context.SaveChangesAsync();
    }
}

public class CachedCartRepository(IUnitOfWork unitOfWork, IDistributedCache cache) : ICartRepository
{
    private static string CacheKey (Guid cartId) => $"cart:{cartId}";
    private static string CacheUserKey (Guid userId) => $"user:{userId}";
    public async Task<IEnumerable<Model.Cart>> GetBoughtCartsAsync(Guid userId) 
        => await unitOfWork.CartRepository.GetBoughtCartsAsync(userId);
    public async Task<Model.Cart?> GetNotBoughtCartByUserIdAsync(Guid userId)
    {
        var cachedUser = await cache.GetRecordAsync<CachedUserDto>(CacheUserKey(userId));
        if (cachedUser == null)
        {
            var cart = await unitOfWork.CartRepository.GetNotBoughtCartByUserIdAsync(userId);
            if (cart == null) return null;
            var newCachedUser = new CachedUserDto(userId, cart.Id);
            await cache.SetRecordAsync(CacheUserKey(userId), newCachedUser);
            cachedUser = await cache.GetRecordAsync<CachedUserDto>(CacheUserKey(userId));
        }
        return await GetCartByIdAsync(cachedUser!.Value.CartId);
    }

    public async Task<Model.Cart?> GetCartByIdAsync(Guid cartId)
    {
        var cachedCart = await cache.GetRecordAsync<Model.Cart>(CacheKey(cartId));
        if (cachedCart == null)
        {
            
            var cart = await unitOfWork.CartRepository.GetCartByIdAsync(cartId);
            if (cart == null) return null;
            await cache.SetRecordAsync(CacheKey(cartId), cart);
            cachedCart = await cache.GetRecordAsync<Model.Cart>(CacheKey(cartId));
        }
        if (cachedCart == null) throw new NotFoundException("Cached cart not found, cache is not working");
        return cachedCart.Value;
    }
    public async Task<Model.Cart> AddCartAsync(Guid userId)
    {
        var newCart = await unitOfWork.CartRepository.AddCartAsync(userId);
        await cache.SetRecordAsync(CacheKey(newCart.Id), newCart);
        return newCart;
    }

    public async Task<Model.Cart> AddPlaceToCart(Guid userId, Guid placeId) 
        => await unitOfWork.CartRepository.AddPlaceToCart(userId, placeId);
    public async Task<Model.Cart> ConfirmCartAsync(Guid userId) 
        => await unitOfWork.CartRepository.ConfirmCartAsync(userId);
    public async Task<Model.Cart> UnConfirmCartAsync(Guid userId) 
        => await unitOfWork.CartRepository.UnConfirmCartAsync(userId);
    public async Task<Model.Cart> BuyCartAsync(Guid cartId) 
        => await unitOfWork.CartRepository.BuyCartAsync(cartId);
    public async Task<Model.Cart> BuyBackCartAsync(Guid cartId) 
        => await unitOfWork.CartRepository.BuyBackCartAsync(cartId);
    public async Task UpdateAmountToPayAsync(Guid cartId, IEnumerable<(int productCost, int productQuantity)>? costCollection)
    {
        var cart = await GetCartByIdAsync(cartId);
        if (cart == null) throw new NotFoundException("Cart not found");
        if (costCollection == null) throw new NotFoundException("Cart has no orders");
        cart.AmountToPay = cart.UpdateAmountToPay(costCollection);
        await cache.SetRecordAsync(CacheKey(cartId), cart, isSynced: false);
    }
    public async Task DeleteCartAsync(Guid cartId)
    {
        var foundItem = await GetCartByIdAsync(cartId);
        if (foundItem == null) throw new NotFoundException("Cart not found");
        await cache.RemoveAsync(CacheKey(foundItem.Id));
    }
    public async Task UpdateCartAsync(Model.Cart cart) 
        => await cache.SetRecordAsync(CacheKey(cart.Id), cart, isSynced: true);
}