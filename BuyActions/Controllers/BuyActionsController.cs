using Microsoft.AspNetCore.Mvc;
using Models;

namespace BuyActions.Controllers;

[ApiController]
[Route("[controller]")]
public class BuyActionsController(IBuyService buyService) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<BuyReport>> Get() => await buyService.Get();
    [HttpGet("{reportId:guid}")]
    public async Task<BuyReport?> Get(Guid reportId) => await buyService.Get(reportId);
    [HttpPost("BuyCart")]
    public async Task BuyCart(Cart cart) => await buyService.BuyCart(cart);
}