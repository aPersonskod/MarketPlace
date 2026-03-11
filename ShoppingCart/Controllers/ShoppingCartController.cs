using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public async Task<IActionResult> GetCart()
    {
        try
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            return Ok(await shoppingCart.GetCart(token));
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
    [Authorize]
    public async Task<IActionResult> ConfirmCart(Guid placeId)
    {
        try
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            return Ok(await shoppingCart.ConfirmAndBuyCart(placeId, token));
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
    [Authorize]
    public async Task<IActionResult> AddOrder(Guid productId, int quantity)
    {
        try
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            return Ok(await shoppingCart.AddOrder(productId, quantity, token));
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpDelete("[action]")]
    [Authorize]
    public async Task<IActionResult> DeleteOrder(Guid productId)
    {
        try
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            return Ok(await shoppingCart.DeleteOrder(productId, token));
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
}