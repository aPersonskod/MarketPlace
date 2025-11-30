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
            return BadRequest(e.Message);
        }
    }

    [HttpPost("BuyCart")]
    public async Task BuyCart(Cart cart)
    {
        try
        {
            await buyService.BuyCart(cart);
            Ok();
        }
        catch (Exception e)
        {
            BadRequest(e.Message);
        }
    }
}