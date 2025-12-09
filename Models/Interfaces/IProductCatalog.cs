using Models.Dtos;

namespace Models.Interfaces;

public interface IProductCatalog
{
    Task<IEnumerable<Product>> Get();
    Task<Product> Get(Guid id);
}