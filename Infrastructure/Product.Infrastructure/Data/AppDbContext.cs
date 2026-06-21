using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Product.Infrastructure.Data;

public class AppDbContext : DbContext
{
    private readonly ILogger<AppDbContext> _logger;
    public AppDbContext(DbContextOptions<AppDbContext> options, ILogger<AppDbContext> logger) : base(options)
    {
        _logger = logger;
        Database.EnsureCreated(); // need to be comment when create/apply migration
    }

    public DbSet<Model.Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Model.Product>().HasData(
            new Model.Product { Id = new Guid("ca173802-206e-4a88-a6cc-2ac93e590fba"), Name = "Футболка", Cost = 100 },
            new Model.Product { Id = new Guid("304e34b1-5267-433a-8d7d-a0abd761da11"), Name = "Шорты", Cost = 200 },
            new Model.Product { Id = new Guid("35e52d12-62f8-4451-ab81-b549fa3f066b"), Name = "Носки", Cost = 50 },
            new Model.Product { Id = new Guid("388ea4e6-f760-4735-9aa5-e3df9906b49c"), Name = "Трусы", Cost = 70 }
        );
        _logger.LogInformation("Database was created !!!");
    }
}