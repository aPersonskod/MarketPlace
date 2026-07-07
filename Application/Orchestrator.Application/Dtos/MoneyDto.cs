using System.Text.Json.Serialization;

namespace Orchestrator.Application.Dtos;

public class MoneyDto
{
    [JsonPropertyName("money")]
    public int Money { get; set; }
}