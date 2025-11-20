using System.Text.Json.Serialization;

namespace Models;

public class Product : ICloneable
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("name")]
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