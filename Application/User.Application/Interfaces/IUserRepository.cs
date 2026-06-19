using Models.Interfaces;
using User.Application.Dto;

namespace User.Application.Interfaces;

public interface IUserRepository : IRepository<Models.User>
{
    Task<Models.User?> Authorize(UserCredentialsDto credentials);
    Task<Models.User> WalletReplenishment(UserMoneyDto userMoneyDto);
    Task<Models.User> SpendMoney(UserMoneyDto userMoneyDto);
}