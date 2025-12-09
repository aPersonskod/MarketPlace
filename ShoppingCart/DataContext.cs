using Microsoft.EntityFrameworkCore;
using Models;
using Models.Dtos;

namespace ShoppingCart;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Cart> ShoppingCarts { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Place> Places { get; set; }
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