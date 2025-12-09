using Models.Dtos;
using Models.Interfaces;

namespace ProductCatalog.Services;

public class ProductCatalogService(DataContext dataContext) : IProductCatalog
{
    public async Task<IEnumerable<ProductDto>> Get() => await Task.FromResult(dataContext.Products);
    public async Task<ProductDto> Get(Guid id)
    {
        var product = await dataContext.Products.FindAsync(id);
        if (product == null) throw new Exception("Product not found");
        return await Task.FromResult(product);
    }
}