namespace Models;

public class Cart
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? PlaceId { get; set; }
    public int AmountToPay { get; set; }
    public bool IsConfirmed { get; set; }
    public bool IsBought { get; set; }
}