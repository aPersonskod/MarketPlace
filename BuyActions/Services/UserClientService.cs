using BuyActions.Settings;
using Microsoft.Extensions.Options;
using Models;

namespace BuyActions.Services;

public class UserClientService(IOptions<UserSettings> userOptions)
{
    private readonly string _baseAddress = userOptions.Value.Address;
    public async Task<UserDto> GetUser(Guid userId)
    {
        var query = $"{_baseAddress}/{userId}";
        return (await query.GetQuery<UserDto>())!;
    }

    public async Task<UserDto> SpendMoney(Guid userId, int money)
    {
        var query = $"{_baseAddress}/SpendMoney?userId={userId}&money={money}";
        return (await query.PostQuery<UserDto>())!;
    }
}