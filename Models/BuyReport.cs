namespace Models;

public class BuyReport
{
    private readonly DateTime _saleDate;
    public Guid Id { get; set; }
    public Guid CartId { get; set; }
    public DateTime SaleDate
    {
        get => _saleDate;
        init => _saleDate = value.ToUniversalTime();
    }
}