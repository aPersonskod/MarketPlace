using Model.SharedExceptions;
using Product.Application.Dtos;
using Product.Application.Interfaces;
using Product.Application.Mappings;

namespace Product.Application.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    public async Task<IEnumerable<ProductDto>> Get()
    {
        var products = await productRepository.GetProducts();
        return products.Select(x => x.ToDto());
    }

    public async Task<ProductDto?> Get(Guid productId)
    {
        var product = await productRepository.GetProductById(productId);
        return product.ToDto();
    }
}