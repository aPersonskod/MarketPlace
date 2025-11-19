using Microsoft.AspNetCore.Mvc;
using Models;

namespace ShoppingCart.Controllers;

[ApiController]
[Route("[controller]")]
public class ShoppingCartController(IShoppingCart shoppingCart) : ControllerBase
{
    [HttpGet]
    public async Task<Cart> Get() => await shoppingCart.Get();
    
    [HttpPut]
    public async Task<Cart> AddOrder(Guid productId, int quantity) => await shoppingCart.AddOrder(productId, quantity);
    
    [HttpDelete]
    public async Task<Cart> DeleteOrder(Guid productId) => await shoppingCart.DeleteOrder(productId);
}