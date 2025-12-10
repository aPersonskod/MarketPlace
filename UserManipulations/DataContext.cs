using Microsoft.EntityFrameworkCore;
using Models;

namespace UserManipulations;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User { 
                Id = new Guid("49792511-261b-4edb-94a5-ecb8540e60ff"),
                Name = "Петя Пяточкин",
                Email = "patochin@gmail.com",
                Password = "12345"
            }
        );
    }
}

public class StaticData
{
    public static List<User> Users { get; set; } =
    [
        new User()
        {
            Id = Guid.NewGuid(),
            Name = "Петя Пяточкин",
            Email = "patochin228@gmail.com",
            Password = "12345"
        },
        new User()
        {
            Id = Guid.NewGuid(),
            Name = "Андрей Викторович",
            Email = "the_king@gmail.com",
            Password = "password"
        }
    ];
}