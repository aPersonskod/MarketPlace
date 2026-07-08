using Cart.Application.Dtos;

namespace Cart.Application.Interfaces.Services;

public interface IProductService
{
    Task<ProductDto> GetProductByIdAsync(Guid productId);
}