using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Interfaces;

namespace ProductCatalog.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductCatalogController(IProductCatalog productCatalog) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<Product>> Get() => await productCatalog.Get();

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        try
        {
            return Ok(await productCatalog.Get(id));
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
}