using BuyActions.Commands;
using BuyActions.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos;

namespace BuyActions.Controllers;

[ApiController]
[Route("[controller]")]
public class BuyActionsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async IAsyncEnumerable<BuyReportDto?> Get()
    {
        var reports = await mediator.Send(new GetBuyReportsQuery());
        await foreach (var buyReportDto in reports) yield return buyReportDto;
    }

    [HttpGet("{reportId:guid}")]
    public async Task<IActionResult> Get(Guid reportId)
    {
        try
        {
            return Ok(await mediator.Send(new GetBuyReportByIdQuery(reportId)));
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
    
    [HttpGet("[action]")]
    public async Task<IEnumerable<BuyReportDto?>> GetByUserId(Guid userId) => await mediator.Send(new GetBuyReportByUserIdQuery(userId));

    [HttpPost("[action]")]
    public async Task<IActionResult> BuyCart(CartDto cartDto)
    {
        try
        {
            await mediator.Send(new BuyCartCommand(cartDto));
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
}