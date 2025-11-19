namespace Models;

public class Order : ICloneable
{
    public Guid Id { get; set; }
    public Product? OrderedProduct { get; set; }
    public int Quantity { get; set; }
    public object Clone()
    {
        return new Order()
        {
            Id = Id,
            OrderedProduct = OrderedProduct?.Clone() as Product,
            Quantity = Quantity
        };
    }
}