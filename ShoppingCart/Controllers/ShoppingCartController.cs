using Microsoft.AspNetCore.Mvc;
using Models.Dtos;
using Models.Interfaces;

namespace ShoppingCart.Controllers;

[ApiController]
[Route("[controller]")]
public class ShoppingCartController(IShoppingCart shoppingCart) : ControllerBase
{
    [HttpGet("[action]")]
    public async Task<IEnumerable<PlaceDto>> GetPlaces() => await shoppingCart.GetPlaces();
    
    [HttpGet("[action]")]
    public async Task<PlaceDto> GetPlace(Guid placeId) => await shoppingCart.GetPlace(placeId); // todo IActionResult

    [HttpGet("[action]")]
    public async Task<IActionResult> GetCart(Guid userId)
    {
        try
        {
            return Ok(await shoppingCart.GetCart(userId));
        }
        catch (Exception e)
        {
            return NotFound(new { message = e.Message });
        }
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> GetCartById(Guid cartId)
    {
        try
        {
            return Ok(await shoppingCart.GetCartById(cartId));
        }
        catch (Exception e)
        {
            return NotFound(new { message = e.Message });
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
    
    [HttpGet("[action]")]
    public async Task<IActionResult> GetCartOrders(Guid cartId)
    {
        try
        {
            return Ok(await shoppingCart.GetOrders(cartId));
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