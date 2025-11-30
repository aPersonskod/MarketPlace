namespace Models;

public class BuyReport
{
    public Guid Id { get; set; }
    public Cart Cart { get; set; }
    public DateTime SaleDate { get; set; }
}