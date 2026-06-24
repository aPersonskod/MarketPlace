using BuyReport.Application.Dtos;
using BuyReport.Application.Interfaces;
using BuyReport.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Model.Extensions;

namespace BuyReport.Infrastructure.Repositories;

public class UserRepository(IOptions<UserSettings> userOptions) : IUserRepository
{
    private readonly string _baseUrl = userOptions.Value.Address + "/api/user-service";
    public async Task<UserDto?> GetUserAsync(string? authToken)
    {
        return await _baseUrl.GetQuery<UserDto>(authToken);
    }
}