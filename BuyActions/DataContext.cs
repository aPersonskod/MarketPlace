using Microsoft.EntityFrameworkCore;
using Models;
using Models.Dtos;

namespace BuyActions;

public sealed class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) => Database.EnsureCreated();
    public DbSet<BuyReport> BuyReports { get; set; }
}

public class StaticData
{
    public static List<BuyReportDto> BuyReports { get; set; } = [];
}