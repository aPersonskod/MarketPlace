using Models;

namespace ShoppingCart;

public class DataContext
{
    private readonly StaticData _staticData = new();

    public List<Cart> ShoppingCarts { get; set; } = new List<Cart>();
    public List<Order> Orders { get => StaticData.Orders; set => StaticData.Orders = value; }
    public List<CartEvent> CartEvents { get; set; } = new List<CartEvent>();
}

public class StaticData
{
    public static List<Order> Orders { get; set; } = [];
}