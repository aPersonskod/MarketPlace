using System.Text.Json.Serialization;

namespace User.Application.Dto;

public class MoneyDto
{
    [JsonPropertyName("money")]
    public int Money { get; set; }
}
public class UserMoneyDto : MoneyDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
}

