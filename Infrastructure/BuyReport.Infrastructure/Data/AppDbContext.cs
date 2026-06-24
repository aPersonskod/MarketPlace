using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BuyReport.Infrastructure.Data;

public class AppDbContext : DbContext
{
    private readonly ILogger<AppDbContext> _logger;
    public AppDbContext(DbContextOptions<AppDbContext> options, ILogger<AppDbContext> logger) : base(options)
    {
        _logger = logger;
        //Database.EnsureCreated(); // need to be comment when create/apply migration
    }
    public DbSet<Model.BuyReport> BuyReports { get; set; }
}