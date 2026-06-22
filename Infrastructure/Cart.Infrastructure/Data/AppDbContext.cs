using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;

namespace Cart.Infrastructure.Data;

public class AppDbContext : DbContext
{
    private readonly ILogger<AppDbContext> _logger;
    public AppDbContext(DbContextOptions<AppDbContext> options, ILogger<AppDbContext> logger) : base(options)
    {
        _logger = logger;
        Database.EnsureCreated(); // need to be comment when create/apply migration
    }

    public DbSet<Model.Cart> ShoppingCarts { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Place> Places { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Place>().HasData(
            new Place(){Id = new Guid("f853bb36-6ad3-4d03-ad7e-9a3545d21429"), Address = "Яхтенная ул., 3, корп. 2", WorkingTime = "10:00 - 22:00"},
            new Place(){Id = new Guid("98eac40c-77e6-44c8-8165-b9380b59a37b"), Address = "6-я Советская улица, 37", WorkingTime = "09:00 - 21:00"}
        );
        _logger.LogInformation("Database was created !!!");
    }
}