using Microsoft.EntityFrameworkCore;
using Models;

namespace UserManipulations;

public sealed class DataContext : DbContext
{
    private readonly ILogger<DataContext> _logger;
    public DbSet<User> Users { get; set; }

    public DataContext(DbContextOptions<DataContext> options, ILogger<DataContext> logger) : base(options)
    {
        _logger = logger;
        Database.EnsureCreated();
    }
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
        _logger.LogInformation("Database was created !!!");
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