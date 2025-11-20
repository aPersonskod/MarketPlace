using Models;

namespace ShoppingCart;

public class DataContext
{
    public List<Cart> ShoppingCarts { get => StaticData.ShoppingCarts; set => StaticData.ShoppingCarts = value; }
    public List<Order> Orders { get => StaticData.Orders; set => StaticData.Orders = value; }
}

internal class StaticData
{
    public static List<Order> Orders { get; set; } = [];
    public static List<Cart> ShoppingCarts { get; set; } = [];
}