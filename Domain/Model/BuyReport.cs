namespace Model;

public class BuyReport
{
    public Guid Id { get; set; }
    public Guid CartId { get; set; }
    public DateTime SaleDate { get; set; }

    public static BuyReport Create(Guid cartId, DateTime saleDate)
    {
        return new BuyReport()
        {
            Id = Guid.NewGuid(),
            CartId = cartId,
            SaleDate = saleDate.ToUniversalTime()
        };
    }
}