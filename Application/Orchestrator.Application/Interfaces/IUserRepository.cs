using Orchestrator.Application.Dtos;

namespace Orchestrator.Application.Interfaces;

public interface IUserRepository
{
    Task<UserDto> WalletReplenishment(UserMoneyDto userMoneyDto);
    Task<UserDto> SpendMoney(UserMoneyDto userMoneyDto);
}