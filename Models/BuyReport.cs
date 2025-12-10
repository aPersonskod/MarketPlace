namespace Models;

public class BuyReport
{
    private DateTime _saleDate;
    public Guid Id { get; set; }
    public Guid CartId { get; set; }
    public DateTime SaleDate
    {
        get => _saleDate;
        set => _saleDate = value.ToUniversalTime();
    }
}