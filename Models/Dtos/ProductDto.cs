using System.Text.Json.Serialization;

namespace Models.Dtos;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Cost { get; set; }
}

public class ProductDto : ICloneable
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("cost")]
    public int Cost { get; set; }
    public object Clone()
    {
        return new ProductDto()
        {
            Id = Id,
            Name = Name
        };
    }
}