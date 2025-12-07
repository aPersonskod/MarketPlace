using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Interfaces;

namespace ShoppingCart.Controllers;

[ApiController]
[Route("[controller]")]
public class ShoppingCartController(IShoppingCart shoppingCart) : ControllerBase
{
    [HttpGet("[action]")]
    public async Task<IEnumerable<Place>> GetPlaces() => await shoppingCart.GetPlaces();

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> Get(Guid userId)
    {
        try
        {
            return Ok(await shoppingCart.Get(userId));
        }
        catch (Exception e)
        {
            return NotFound(new { message = e.Message });
        }
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> AddOrder(Guid userId, Guid productId, int quantity)
    {
        try
        {
            return Ok(await shoppingCart.AddOrder(userId, productId, quantity));
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> ConfirmCart(Guid userId, Guid placeId)
    {
        try
        {
            return Ok(await shoppingCart.ConfirmAndBuyCart(userId, placeId));
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> MarkCartAsBought(Guid cartId)
    {
        try
        {
            await shoppingCart.MarkCartAsBought(cartId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpDelete("[action]")]
    public async Task<IActionResult> DeleteOrder(Guid userId, Guid productId)
    {
        try
        {
            return Ok(await shoppingCart.DeleteOrder(userId, productId));
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
}