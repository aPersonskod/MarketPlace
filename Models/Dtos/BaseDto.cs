using System.Text.Json.Serialization;

namespace Models.Dtos;

public class BaseDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
}