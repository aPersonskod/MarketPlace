using Models;

namespace ProductCatalog.Services;

public class ProductCatalogService(DataContext dataContext) : IProductCatalog
{
    public async Task<IEnumerable<Product>> Get() => await Task.FromResult(dataContext.Products);
    public async Task<Product?> Get(Guid id) => await Task.FromResult(dataContext.Products.FirstOrDefault(p => p.Id == id));
}