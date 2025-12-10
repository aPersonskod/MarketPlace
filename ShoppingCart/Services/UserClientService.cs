using Microsoft.Extensions.Options;
using Models.Dtos;
using Models.Extensions;
using ShoppingCart.Settings;

namespace ShoppingCart.Services;

public class UserClientService(IOptions<UserSettings> userSettings)
{
    private readonly string _baseAddress = userSettings.Value.Address;
    public async Task<UserDto?> GetUser(Guid userId)
    {
        var query = $"{_baseAddress}/{userId}";
        try
        {
            var result = await query.GetQuery<UserDto>();
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}