using Microsoft.EntityFrameworkCore;
using Models;
using Models.Dtos;

namespace ProductCatalog;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<ProductDto> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductDto>().HasData(
            new ProductDto { Id = new Guid("ca173802-206e-4a88-a6cc-2ac93e590fba"), Name = "Футболка", Cost = 100 },
            new ProductDto { Id = new Guid("304e34b1-5267-433a-8d7d-a0abd761da11"), Name = "Шорты", Cost = 200 },
            new ProductDto { Id = new Guid("35e52d12-62f8-4451-ab81-b549fa3f066b"), Name = "Носки", Cost = 50 },
            new ProductDto { Id = new Guid("388ea4e6-f760-4735-9aa5-e3df9906b49c"), Name = "Трусы", Cost = 70 }
        );
    }
}

internal class StaticData
{
    public static List<ProductDto> Products { get; set; } =
    [
        new ProductDto { Id = new Guid("ca173802-206e-4a88-a6cc-2ac93e590fba"), Name = "Футболка", Cost = 100 },
        new ProductDto { Id = new Guid("304e34b1-5267-433a-8d7d-a0abd761da11"), Name = "Шорты", Cost = 200 },
        new ProductDto { Id = new Guid("35e52d12-62f8-4451-ab81-b549fa3f066b"), Name = "Носки", Cost = 50 },
        new ProductDto { Id = new Guid("388ea4e6-f760-4735-9aa5-e3df9906b49c"), Name = "Трусы", Cost = 70 },
    ];
}