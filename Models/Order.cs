namespace Models;

public class Order
{
    public Guid Id { get; set; }
    public Guid CartId { get; set; }
    public Guid OrderedProductId { get; set; }
    public int Quantity { get; set; }
}