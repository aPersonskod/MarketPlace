using Models;
using Models.Interfaces;

namespace ProductCatalog.Services;

public class ProductCatalogService(DataContext dataContext) : IProductCatalog
{
    public async Task<IEnumerable<Product>> Get() => await Task.FromResult(dataContext.Products);
    public async Task<Product> Get(Guid id)
    {
        var product = dataContext.Products.FirstOrDefault(p => p.Id == id);
        if (product == null) throw new Exception("Product not found");
        return await Task.FromResult(product);
    }
}