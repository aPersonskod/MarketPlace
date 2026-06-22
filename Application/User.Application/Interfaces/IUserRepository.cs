using User.Application.Dto;

namespace User.Application.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<Model.User>> GetAllAsync();
    Task<Model.User?> GetByIdAsync(Guid id);
    Task<Model.User> AddAsync(CreateUserDto userDto);
    Task DeleteAsync(Guid id);
    Task<Model.User?> Authorize(UserCredentialsDto credentials);
    Task<Model.User> WalletReplenishment(UserMoneyDto userMoneyDto);
    Task<Model.User> SpendMoney(UserMoneyDto userMoneyDto);
}