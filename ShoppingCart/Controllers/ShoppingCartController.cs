using Microsoft.AspNetCore.Mvc;
using Models;

namespace ShoppingCart.Controllers;

[ApiController]
[Route("[controller]")]
public class ShoppingCartController(IShoppingCart shoppingCart) : ControllerBase
{
    [HttpGet("Places")]
    public async Task<IEnumerable<Place>> Get() => await shoppingCart.GetPlaces();
    [HttpGet]
    public async Task<Cart> Get(Guid userId) => await shoppingCart.Get(userId);
    
    [HttpPost]
    public async Task<Cart> AddOrder(Guid userId, Guid productId, Guid placeId, int quantity) =>
        await shoppingCart.AddOrder(userId, productId, placeId, quantity);
    
    [HttpDelete]
    public async Task<IActionResult> DeleteOrder(Guid userId, Guid productId)
    {
        await shoppingCart.DeleteOrder(userId, productId);
        return Ok();
    }
}