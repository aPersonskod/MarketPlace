using Models;
using Models.Dtos;
using Models.Interfaces;

namespace ProductCatalog.Services;

public class ProductCatalogService(DataContext dataContext) : IProductCatalog
{
    public async Task<IEnumerable<ProductDto>> Get() => await Task.FromResult(dataContext.Products.Select(GetProductDto));
    public async Task<ProductDto> Get(Guid id)
    {
        var product = await dataContext.Products.FindAsync(id);
        if (product == null) throw new Exception("Product not found");
        return await Task.FromResult(GetProductDto(product));
    }
    
    private ProductDto GetProductDto(Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Cost = product.Cost
    };
}