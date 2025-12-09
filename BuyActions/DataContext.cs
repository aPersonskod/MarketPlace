using Models;
using Models.Dtos;

namespace BuyActions;

public class DataContext
{
    public List<BuyReportDto> BuyReports { get => StaticData.BuyReports; set => StaticData.BuyReports = value; }
}

public class StaticData
{
    public static List<BuyReportDto> BuyReports { get; set; } = [];
}