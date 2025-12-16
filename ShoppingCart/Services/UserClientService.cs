using Microsoft.Extensions.Options;
using Models.Dtos;
using Models.Extensions;
using ShoppingCart.Settings;

namespace ShoppingCart.Services;

public class UserClientService(IOptions<UserSettings> userSettings, ILogger<UserClientService> logger)
{
    private readonly string _baseAddress = userSettings.Value.HttpAddress;
    public async Task<UserDto?> GetUser(Guid userId)
    {
        var query = $"{_baseAddress}/{userId}";
        logger.LogInformation(query);
        try
        {
            var result = await query.GetQuery<UserDto>();
            logger.LogInformation($"user: {result?.Name}");
            return result;
        }
        catch (Exception e)
        {
            logger.LogInformation($"user exception: {e?.Message}");
            Console.WriteLine(e);
            return null;
        }
    }
}