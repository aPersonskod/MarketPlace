using BuyActions.Settings;
using Microsoft.Extensions.Options;
using Models;

namespace BuyActions.Services;

public class UserClientService(IOptions<UserSettings> userOptions)
{
    private readonly string _baseAddress = userOptions.Value.Address;
    public async Task<User> GetUser(Guid userId)
    {
        var query = $"{_baseAddress}/{userId}";
        return (await query.GetQuery<User>())!;
    }

    public async Task<User> SpendMoney(Guid userId, int money)
    {
        var query = $"{_baseAddress}/SpendMoney?userId={userId}&money={money}";
        return (await query.PostQuery<User>())!;
    }
}