using BuyActions.Commands;
using BuyActions.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos;

namespace BuyActions.Controllers;

[ApiController]
[Route("[controller]")]
public class BuyActionsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async IAsyncEnumerable<BuyReportDto?> Get()
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var reports = await mediator.Send(new GetBuyReportsQuery(token!));
        await foreach (var buyReportDto in reports) yield return buyReportDto;
    }

    [HttpGet("{reportId:guid}")]
    [Authorize]
    public async Task<IActionResult> Get(Guid reportId)
    {
        try
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            return Ok(await mediator.Send(new GetBuyReportByIdQuery(reportId, token!)));
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpGet("[action]")]
    [Authorize]
    public async Task<IEnumerable<BuyReportDto?>> GetByUserId()
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        return await mediator.Send(new GetBuyReportByUserIdQuery(token!));
    }

    [HttpPost("[action]")]
    [Authorize]
    public async Task<IActionResult> BuyCart(CartDto cartDto)
    {
        try
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            await mediator.Send(new BuyCartCommand(cartDto, token!));
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
}