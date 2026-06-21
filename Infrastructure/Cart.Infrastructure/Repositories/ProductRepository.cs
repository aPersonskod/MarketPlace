using Cart.Application.Interfaces.Repositories;
using Cart.Infrastructure.Data;
using Product.Application.Dtos;

namespace Cart.Infrastructure.Repositories;

public class ProductRepository(AppDbContext context) : IProductRepository
{
    public Task<ProductDto> GetProductByIdAsync(Guid productId)
    {
        throw new NotImplementedException();
    }
}