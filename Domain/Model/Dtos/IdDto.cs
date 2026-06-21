using System.Text.Json.Serialization;

namespace Model.Dtos;

public class IdDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
}