using Model.Interfaces;
using User.Application.Dto;

namespace User.Application.Interfaces;

public interface IUserRepository : IRepository<Model.User>
{
    Task<Model.User?> Authorize(UserCredentialsDto credentials);
    Task<Model.User> WalletReplenishment(UserMoneyDto userMoneyDto);
    Task<Model.User> SpendMoney(UserMoneyDto userMoneyDto);
}