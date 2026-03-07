using Microsoft.Extensions.Caching.Distributed;
using Models;
using Models.Dtos;
using Models.Extensions;
using Models.Interfaces;

namespace ProductCatalog.Services;

public class ProductCatalogService(DataContext dataContext, IDistributedCache cache) : IProductCatalog
{
    public async Task<IEnumerable<ProductDto>> Get() => await Task.FromResult(dataContext.Products.Select(GetProductDto));
    public async Task<ProductDto> Get(Guid id)
    {
        var cachedProduct = await cache.GetRecordAsync<Product>(id.ToString());
        if (cachedProduct == null)
        {
            cachedProduct = await dataContext.Products.FindAsync(id);
            if (cachedProduct == null) throw new Exception("Product not found");
            await cache.SetRecordAsync(id.ToString(), cachedProduct, TimeSpan.FromMinutes(10));
        }

        return await Task.FromResult(GetProductDto(cachedProduct));
    }
    
    private ProductDto GetProductDto(Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Cost = product.Cost
    };
}