using Microsoft.Extensions.Options;
using Models;
using ShoppingCart.Settings;

namespace ShoppingCart.Services;

public class UserClientService(IOptions<UserSettings> userSettings)
{
    private readonly string _baseAddress = userSettings.Value.Address;
    public async Task<User?> GetUser(Guid userId)
    {
        var query = $"{_baseAddress}/{userId}";
        try
        {
            var result = await query.GetQuery<User>();
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}