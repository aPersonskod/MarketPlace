namespace Models;

public class Product : ICloneable
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public object Clone()
    {
        return new Product()
        {
            Id = Id,
            Name = Name
        };
    }
}