namespace User.Application.Interfaces;

public interface IAuthRepository
{
    Task<Model.Token> CreateRefreshToken(Guid userId, string role);
    string CreateAccessToken(Guid userId, string role);
    Task<Model.Token> RefreshToken(string refreshToken);
}