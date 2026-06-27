namespace Model;

public class Cart
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? PlaceId { get; set; }
    public int AmountToPay { get; set; }
    public bool IsConfirmed { get; set; }
    public bool IsBought { get; set; }

    public static Cart CreateCart(Guid userId) => new Cart()
    {
        Id = Guid.NewGuid(),
        UserId = userId
    };
    
    // Не подтвержденная тележка
    public bool IsUnverified(Guid userId) => UserId == userId && !IsConfirmed && !IsBought;
    public void ConfirmCart()
    {
        if (PlaceId == null) throw new ArgumentException("Place is required");
        if (AmountToPay <= 0) throw new ArgumentException("Cart can not be empty");
        IsConfirmed = true;
    }

    public void BuyCart()
    {
        if (!IsConfirmed) throw new ArgumentException("Cart is not confirmed");
        IsBought = true;
    }

    public int UpdateAmountToPay(IEnumerable<(int productCost, int productQuantity)> costCollection)
    {
        AmountToPay = 0;
        foreach (var valueTuple in costCollection) AmountToPay += valueTuple.productCost * valueTuple.productQuantity;
        return AmountToPay;
    }
}