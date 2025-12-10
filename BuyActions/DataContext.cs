using Microsoft.EntityFrameworkCore;
using Models;
using Models.Dtos;

namespace BuyActions;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<BuyReport> BuyReports { get; set; }
}

public class StaticData
{
    public static List<BuyReportDto> BuyReports { get; set; } = [];
}