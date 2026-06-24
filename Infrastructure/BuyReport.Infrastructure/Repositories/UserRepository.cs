using BuyReport.Application.Dtos;
using BuyReport.Application.Interfaces;
using BuyReport.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Model.Extensions;

namespace BuyReport.Infrastructure.Repositories;

public class UserRepository(IOptions<UserSettings> userOptions) : IUserRepository
{
    public async Task<UserDto?> GetUserAsync(string? authToken)
    {
        var query = $"{userOptions.Value.Address}";
        return await query.GetQuery<UserDto>(authToken);
    }
}