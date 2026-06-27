using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;

namespace User.Infrastructure.Data;

public class AppDbContext : DbContext
{
    private readonly ILogger<AppDbContext> _logger;
    public DbSet<Model.User> Users { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options, ILogger<AppDbContext> logger) : base(options)
    {
        _logger = logger;
        Database.EnsureCreated(); // need to be comment when create/apply migration
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Model.User>().HasData(
            new Model.User { 
                Id = new Guid("49792511-261b-4edb-94a5-ecb8540e60ff"),
                Name = "Петя Пяточкин",
                Email = "patochin@gmail.com",
                Password = "12345",
                Role = Role.user.ToString()
            },
            new Model.User()
            {
                Id = new Guid("51228188-871a-43a8-91d3-a2595fb42bed"),
                Name = "Test test",
                Email = "test@gmail.com",
                Password = "test_test",
                Role = Role.user.ToString()
            }
        );
        //modelBuilder.Entity<User>().Property(u => u.Role).HasConversion<string>().HasDefaultValue(Role.user);
        _logger.LogInformation("Database was created !!!");
    }
}