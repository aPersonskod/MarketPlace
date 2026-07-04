using Orchestrator.Application.Dtos;
using Orchestrator.Application.Interfaces;

namespace Orchestrator.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    public Task<UserDto> WalletReplenishment(UserMoneyDto userMoneyDto)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> SpendMoney(UserMoneyDto userMoneyDto)
    {
        throw new NotImplementedException();
    }
}