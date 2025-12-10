using Microsoft.EntityFrameworkCore;
using Models;
using Models.Dtos;

namespace ShoppingCart;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Cart> ShoppingCarts { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Place> Places { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Place>().HasData(
            new Place(){Id = new Guid("f853bb36-6ad3-4d03-ad7e-9a3545d21429"), Address = "Яхтенная ул., 3, корп. 2", WorkingTime = "10:00 - 22:00"},
            new Place(){Id = new Guid("98eac40c-77e6-44c8-8165-b9380b59a37b"), Address = "6-я Советская улица, 37", WorkingTime = "09:00 - 21:00"}
        );
    }
}

internal class StaticData
{
    public static List<Cart> ShoppingCarts { get; set; } = [];
    public static List<Place> Places { get; set; } = 
    [
        new Place(){Id = Guid.NewGuid(), Address = "ул. Пушкина, дом Колотушкина", WorkingTime = "10:00 - 20:00"},
        new Place(){Id = Guid.NewGuid(), Address = "Яхтенная ул., 3, корп. 2", WorkingTime = "10:00 - 22:00"}
    ];
}