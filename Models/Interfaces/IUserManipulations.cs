namespace Models;

public interface IUserManipulations
{
    Task<IEnumerable<User>> Get();
    Task<User?> Get(Guid userId);
    Task<User?> Authorize(string email, string password);
    Task<User> Add(User user);
    Task<User?> Update(User user);
    Task Delete(Guid userId);
    Task<User> WalletReplenishment(Guid userId, int money);
    Task<User> SpendMoney(Guid userId, int money);
}