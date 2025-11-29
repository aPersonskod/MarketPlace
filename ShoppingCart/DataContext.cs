using Models;

namespace ShoppingCart;

public class DataContext
{
    public List<Cart> ShoppingCarts { get => StaticData.ShoppingCarts; set => StaticData.ShoppingCarts = value; }
    public List<Place> Places { get => StaticData.Places; set => StaticData.Places = value; }
}

internal class StaticData
{
    public static List<Cart> ShoppingCarts { get; set; } = [];
    public static List<Place> Places { get; set; } = 
    [
        new Place(){Id = Guid.NewGuid(), Address = "ул. Пушкина, дом Колотушкина", WorkingTime = "10:00 - 20:00"},
        new Place(){Id = Guid.NewGuid(), Address = "Яхтенная ул., 3, корп. 2", WorkingTime = "10:00 - 22:00"}
    ];
}