using Microsoft.Extensions.Options;
using Model.Extensions;
using Orchestrator.Application.Dtos;
using Orchestrator.Application.Interfaces;
using Orchestrator.Infrastructure.Settings;

namespace Orchestrator.Infrastructure.Repositories;

public class UserRepository(IOptions<UserSettings> userOptions) : IUserRepository
{
    private readonly string _baseUrl = userOptions.Value.Address + "/api/user-service";
    public async Task<UserDto?> WalletReplenishment(UserMoneyDto userMoneyDto)
    {
        var url = $"{_baseUrl}/top-up-money";
        var moneyDto = new MoneyDto(){ Money = userMoneyDto.Money };
        return await url.PatchQuery<UserDto, MoneyDto>(moneyDto, userMoneyDto.AuthToken);
    }

    public async Task<UserDto?> SpendMoney(UserMoneyDto userMoneyDto)
    {
        var url = $"{_baseUrl}/spend-money";
        var moneyDto = new MoneyDto(){ Money = userMoneyDto.Money };
        return await url.PatchQuery<UserDto, MoneyDto>(moneyDto, userMoneyDto.AuthToken);
    }
}