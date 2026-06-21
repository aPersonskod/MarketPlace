using Microsoft.EntityFrameworkCore;
using Product.Application.Interfaces;
using Product.Infrastructure.Data;

namespace Product.Infrastructure.Repositories;

public class ProductRepository(AppDbContext context) : IProductRepository
{
    public async Task<IEnumerable<Model.Product>> GetProducts() => await context.Products.ToListAsync();
    public async Task<Model.Product?> GetProductById(Guid productId) 
        => await context.Products.FirstOrDefaultAsync(x => x.Id == productId);
}