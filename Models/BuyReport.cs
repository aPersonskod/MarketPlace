namespace Models;

public class BuyReport
{
    public Guid Id { get; set; }
    public Guid CartId { get; set; }
    public DateTime SaleDate { get; set; }
}