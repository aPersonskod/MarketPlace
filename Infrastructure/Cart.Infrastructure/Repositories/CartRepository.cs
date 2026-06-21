using Cart.Application.Interfaces.Repositories;
using Cart.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Model.SharedExceptions;

namespace Cart.Infrastructure.Repositories;

public class CartRepository(AppDbContext context) : ICartRepository
{
    public async Task<IEnumerable<Model.Cart>> GetBoughtCartsAsync(Guid userId) 
        => await context.Carts.Where(x => x.UserId == userId && x.IsBought).ToListAsync();
    public async Task<Model.Cart?> GetCartByUserIdAsync(Guid userId)
    {
        var carts = await context.Carts.Where(x => x.UserId == userId).ToListAsync();
        return carts.FirstOrDefault(x => x.IsUnverified(x.UserId));
    }
    public async Task<Model.Cart> AddCartAsync(Guid userId)
    {
        var cart = Model.Cart.CreateCart(userId);
        await context.Carts.AddAsync(cart);
        return cart;
    }
    public async Task<Model.Cart> ConfirmCartAsync(Guid cartId)
    {
        var cart = await context.Carts.FirstOrDefaultAsync(x => x.Id == cartId);
        if (cart == null) throw new NotFoundException("Cart not found");
        cart.ConfirmCart();
        return cart;
    }
    public async Task<Model.Cart> UnConfirmCartAsync(Guid cartId)
    {
        var cart = await context.Carts.FirstOrDefaultAsync(x => x.Id == cartId);
        if (cart == null) throw new NotFoundException("Cart not found");
        cart.IsConfirmed = false;
        return cart;
    }
    public async Task<Model.Cart> BuyCartAsync(Guid cartId)
    {
        var cart = await context.Carts.FirstOrDefaultAsync(x => x.Id == cartId);
        if (cart == null) throw new NotFoundException("Cart not found");
        cart.BuyCart();
        return cart;
    }
    public async Task<Model.Cart> BuyBackCartAsync(Guid cartId)
    {
        var cart = await context.Carts.FirstOrDefaultAsync(x => x.Id == cartId);
        if (cart == null) throw new NotFoundException("Cart not found");
        cart.IsBought = false;
        return cart;
    }
    public async Task UpdateAmountToPayAsync(Guid cartId, IEnumerable<(int productCost, int productQuantity)> costCollection)
    {
        var cart = await context.Carts.FirstOrDefaultAsync(x => x.Id == cartId);
        if (cart == null) throw new NotFoundException("Cart not found");
        cart.UpdateAmountToPay(costCollection);
    }
    public async Task DeleteCartAsync(Guid cartId)
    {
        var foundItem = await context.Carts.FirstOrDefaultAsync(x => x.Id == cartId);
        if (foundItem == null) throw new NotFoundException("Cart not found");
        context.Carts.Remove(foundItem);
    }
}