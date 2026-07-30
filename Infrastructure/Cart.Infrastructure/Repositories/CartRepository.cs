using Cart.Application.Interfaces.Repositories;
using Cart.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
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