using Models.Dtos;

namespace Models.Interfaces;

public interface IProductCatalog
{
    Task<IEnumerable<ProductDto>> Get();
    Task<ProductDto> Get(Guid id);
}