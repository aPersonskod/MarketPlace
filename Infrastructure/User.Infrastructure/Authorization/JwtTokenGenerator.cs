using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using User.Application.Interfaces;
using User.Infrastructure.Settings;

namespace User.Infrastructure.Authorization;

public class JwtTokenGenerator(IOptions<AuthSettings> authSettings) : IJwtTokenGenerator
{
    public string GenerateJwtToken(Guid id, string role)
    {
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Jti, id.ToString()),
            new Claim(ClaimTypes.Role, role),
        };
        var jwt = new JwtSecurityToken(
            issuer: authSettings.Value.Issuer,
            audience: authSettings.Value.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: new SigningCredentials(authSettings.Value.SecurityKey, SecurityAlgorithms.HmacSha256)
        );
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}