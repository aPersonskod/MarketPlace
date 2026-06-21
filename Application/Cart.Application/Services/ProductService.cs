using Cart.Application.Interfaces.Repositories;
using Cart.Application.Interfaces.Services;
using Product.Application.Dtos;

namespace Cart.Application.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    public async Task<ProductDto> GetProductByIdAsync(Guid productId) 
        => await productRepository.GetProductByIdAsync(productId);
}