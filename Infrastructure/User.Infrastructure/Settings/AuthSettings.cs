using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace User.Infrastructure.Settings;

public class AuthSettings
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Key { get; set; }
    
    public SymmetricSecurityKey SecurityKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
}