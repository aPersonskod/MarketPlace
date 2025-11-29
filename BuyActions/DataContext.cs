using Models;

namespace BuyActions;

public class DataContext
{
    public List<BuyReport> BuyReports { get => StaticData.BuyReports; set => StaticData.BuyReports = value; }
}

public class StaticData
{
    public static List<BuyReport> BuyReports { get; set; } = [];
}