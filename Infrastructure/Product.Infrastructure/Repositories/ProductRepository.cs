using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Model.Extensions;
using Model.SharedExceptions;
using Product.Application.Interfaces;
using Product.Infrastructure.Data;

namespace Product.Infrastructure.Repositories;

public class ProductRepository(AppDbContext context, IDistributedCache cache) : IProductRepository
{
    private static string CacheKey (Guid productId) => $"product:{productId}";
    public async Task<IEnumerable<Model.Product>> GetProducts() => await context.Products.ToListAsync();
    public async Task<Model.Product?> GetProductById(Guid productId)
    {
        var cachedProduct = await cache.GetRecordAsync<Model.Product>(CacheKey(productId));
        if (cachedProduct == null)
        {
            var product = await context.Products.FirstOrDefaultAsync(x => x.Id == productId);
            await cache.SetRecordAsync(CacheKey(productId), product, isSynced: true);
            cachedProduct = await cache.GetRecordAsync<Model.Product>(CacheKey(productId));
        }
        if (cachedProduct == null) throw new NotFoundException("Cached product not found, cache is not working");
        return cachedProduct.Value;
    }
}