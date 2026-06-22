using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Shared.Infrastructure.Settings;

public class AuthSettings
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Key { get; set; }
    
    public SymmetricSecurityKey SecurityKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
}

public static class Extensions
{
    public static AuthCredentials? GetAuthCredentials(this ClaimsPrincipal user)
    {
        var idStr = user.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
        var role = user.FindFirst(ClaimTypes.Role)?.Value;
        if (!Guid.TryParse(idStr, out var userId)) return null;
        if (role == null) return null;
        return new AuthCredentials(userId, role);
    }
}

public struct AuthCredentials(Guid userId, string role)
{
    public Guid UserId { get; private set; } = userId;
    public string Role { get; private set; } = role;
}