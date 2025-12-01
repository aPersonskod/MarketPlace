using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Interfaces;

namespace BuyActions.Controllers;

[ApiController]
[Route("[controller]")]
public class BuyActionsController(IBuyService buyService) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<BuyReport>> Get() => await buyService.Get();
    [HttpGet("{reportId:guid}")]
    public async Task<IActionResult> Get(Guid reportId)
    {
        try
        {
            return Ok(await buyService.Get(reportId));
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> BuyCart(Cart cart)
    {
        try
        {
            await buyService.BuyCart(cart);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
}