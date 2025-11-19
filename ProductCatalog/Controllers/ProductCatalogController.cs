using Microsoft.AspNetCore.Mvc;
using Models;

namespace ProductCatalog.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductCatalogController(IProductCatalog productCatalog) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<Product>> Get() => await productCatalog.Get();

    [HttpGet("{id:guid}")]
    public async Task<Product?> Get(Guid id) => await productCatalog.Get(id);
}