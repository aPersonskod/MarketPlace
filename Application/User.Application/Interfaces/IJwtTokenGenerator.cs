namespace User.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateAccessJwtToken(Guid id, string role);
    string GenerateRefreshToken();
}