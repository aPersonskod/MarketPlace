using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Infrastructure.Settings;
using User.Application.Interfaces;

namespace User.Infrastructure.Authorization;

public class JwtTokenGenerator(IOptions<AuthSettings> authSettings) : IJwtTokenGenerator
{
    public string GenerateAccessJwtToken(Guid id, string role)
    {
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Jti, id.ToString()),
            new Claim(ClaimTypes.Role, role),
        };
        var exp1 = DateTime.UtcNow.AddMinutes(1); // 10
        var exp2 = DateTime.Now.AddMinutes(1);
        var jwt = new JwtSecurityToken(
            issuer: authSettings.Value.Issuer,
            audience: authSettings.Value.Audience,
            claims: claims,
            expires: exp1,
            signingCredentials: new SigningCredentials(authSettings.Value.SecurityKey, SecurityAlgorithms.HmacSha256)
        );
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public string GenerateRefreshToken()
    {
        using var rng = RandomNumberGenerator.Create();
        var randomNumber = new byte[32];
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}