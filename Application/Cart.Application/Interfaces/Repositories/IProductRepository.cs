using Product.Application.Dtos;

namespace Cart.Application.Interfaces.Repositories;

public interface IProductRepository
{
    Task<ProductDto> GetProductByIdAsync(Guid productId);
}