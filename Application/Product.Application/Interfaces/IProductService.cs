using Product.Application.Dtos;

namespace Product.Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> Get();
    Task<ProductDto> Get(Guid productId);
}