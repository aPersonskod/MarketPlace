namespace Models;

public class BuyReport
{
    public Guid Id { get; set; }
    public User User { get; set; }
    public IEnumerable<Order> Orders { get; set; }
    public Place Place { get; set; }
    public DateTime SaleDate { get; set; }
}