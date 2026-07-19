using Microsoft.EntityFrameworkCore;
using Model;
using User.Application.Interfaces;
using User.Infrastructure.Data;
using UnauthorizedAccessException = Model.SharedExceptions.UnauthorizedAccessException;

namespace User.Infrastructure.Repositories;

public class AuthRepository(AppDbContext context, IJwtTokenGenerator jwtTokenGenerator) : IAuthRepository
{
    public async Task<Token> CreateRefreshToken(Guid userId, string role)
    {
        var refreshToken = jwtTokenGenerator.GenerateRefreshToken();
        var token = Token.CreateToken(userId, refreshToken, DateTime.Now);
        var foundToken = await context.Tokens.FirstOrDefaultAsync(x => x.UserId == userId);
        if (foundToken == null)
        {
            await context.Tokens.AddAsync(token);
            await context.SaveChangesAsync();
            return token;
        }
        foundToken.RefreshToken = token.RefreshToken;
        foundToken.Created = token.Created;
        foundToken.Expired = token.Expired;
        await context.SaveChangesAsync();
        return foundToken;
    }

    public string CreateAccessToken(Guid userId, string role) => jwtTokenGenerator.GenerateAccessJwtToken(userId, role);

    public async Task<Token> RefreshToken(string refreshToken)
    {
        var token = await context.Tokens.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
        if (token == null) throw new UnauthorizedAccessException("Invalid refresh token");
        if (token.Expired < DateTime.UtcNow) throw new UnauthorizedAccessException("Refresh token is expired");
        var newToken = Token.CreateToken(token.UserId, jwtTokenGenerator.GenerateRefreshToken(), DateTime.Now);
        //token.RefreshToken = newToken.RefreshToken;
        token.Created = newToken.Created;
        token.Expired = newToken.Expired;
        await context.SaveChangesAsync();
        return token;
    }
}