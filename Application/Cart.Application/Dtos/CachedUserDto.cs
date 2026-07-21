namespace Cart.Application.Dtos;

public class CachedUserDto
{
    public CachedUserDto(Guid userId, Guid cartId)
    {
        UserId = userId;
        CartId = cartId;
    }
    public Guid UserId { get; set; }
    public Guid CartId { get; set; }
}