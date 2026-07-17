using User.Application.Dto;
using User.Application.Interfaces;

namespace User.Application.Services;

public class AuthService(IUserRepository userRepository, IAuthRepository authRepository) : IAuthService
{
    public async Task<TokensData> Authorize(UserCredentialsDto credentials)
    {
        var user = await userRepository.GetByCredentials(credentials);
        if (user == null) throw new UnauthorizedAccessException("Invalid credentials");
        var refreshToken = await authRepository.CreateRefreshToken(user.Id, user.Role);
        var accessToken = authRepository.CreateAccessToken(user.Id, user.Role);
        return new TokensData()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.RefreshToken
        };
    }

    public async Task<TokensData> Refresh(TokensData tokensData)
    {
        var token = await authRepository.RefreshToken(tokensData.RefreshToken);
        var user = await userRepository.GetByIdAsync(token.UserId);
        if (user == null) throw new UnauthorizedAccessException("User with refresh token not found");
        var accessToken = authRepository.CreateAccessToken(user.Id, user.Role);
        return new TokensData()
        {
            AccessToken = accessToken,
            RefreshToken = token.RefreshToken
        };
    }
}