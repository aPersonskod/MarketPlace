namespace Model;

public class Order
{
    public Guid Id { get; set; }
    public Guid CartId { get; set; }
    public Guid OrderedProductId { get; set; }
    public int Quantity { get; set; }

    public static Order CreateOrder(Guid cartId, Guid productId, int quantity) => new Order()
    {
        Id = Guid.NewGuid(),
        CartId = cartId,
        OrderedProductId = productId,
        Quantity = quantity
    };
    
    // condition when deleting
    public bool IsFound(Guid cartId, Guid productId) => CartId == cartId && OrderedProductId == productId;
}