using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Shared.Infrastructure.Extensions;

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