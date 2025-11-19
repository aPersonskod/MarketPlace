namespace Models;

public interface IProductCatalog
{
    Task<IEnumerable<Product>> Get();
    Task<Product?> Get(Guid id);
}