namespace Model;

public class Token
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime Expired { get; set; }
    public static Token CreateToken(Guid userId, string refreshToken, DateTime created)
    {
        return new Token()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            RefreshToken = refreshToken,
            Created = created.ToUniversalTime(),
            Expired = created.AddDays(1).ToUniversalTime()
        };
    }
}