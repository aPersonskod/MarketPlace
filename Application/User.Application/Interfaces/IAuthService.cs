using User.Application.Dto;

namespace User.Application.Interfaces;

public interface IAuthService
{
    Task<TokensData> Authorize(UserCredentialsDto credentials);
    Task<TokensData> Refresh(TokensData tokensData);
}