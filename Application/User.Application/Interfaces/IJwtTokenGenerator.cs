namespace User.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateJwtToken(Guid id, string role);
}