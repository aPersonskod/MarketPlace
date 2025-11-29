using Models;

namespace UserManipulations.Services;

public class UserManipulationsService(DataContext dataContext) : IUserManipulations
{
    public Task<IEnumerable<User>> Get() => Task.FromResult<IEnumerable<User>>(dataContext.Users);

    public Task<User?> Get(Guid userId) => Task.FromResult(dataContext.Users.FirstOrDefault(x => x.Id == userId));

    public Task<User> Add(User user)
    {
        var newUser = new User()
        {
            Id = Guid.NewGuid(),
            Name = user.Name,
            Nick = user.Nick,
            Password = user.Password,
        };
        dataContext.Users.Add(newUser);
        return Task.FromResult(newUser);
    }
    
    public Task<User?> Update(User user)
    {
        var foundUser = dataContext.Users.FirstOrDefault(x => x.Id == user.Id);
        if (foundUser == null) throw new ArgumentNullException("User not found");
        foundUser.Name = user.Name;
        foundUser.Nick = user.Nick;
        foundUser.Password = user.Password;
        return Task.FromResult<User?>(foundUser);

    }

    public Task Delete(Guid userId)
    {
        var foundUser = dataContext.Users.FirstOrDefault(x => x.Id == userId);
        if (foundUser != null) dataContext.Users.Remove(foundUser);
        return Task.FromResult(0);
    }

    public Task<User> WalletReplenishment(Guid userId, int money)
    {
        var foundUser = dataContext.Users.FirstOrDefault(x => x.Id == userId);
        if (foundUser == null) throw new ArgumentNullException("User not found");
        foundUser.Wallet = money;
        return Task.FromResult(foundUser);
    }

    public Task<User> SpendMoney(Guid userId, int money)
    {
        var foundUser = dataContext.Users.FirstOrDefault(x => x.Id == userId);
        if (foundUser == null) throw new ArgumentNullException("User not found");
        if (foundUser.Wallet >= money)
        {
            foundUser.Wallet -= money;
            return Task.FromResult(foundUser);
        }
        throw new Exception("User has not enough money to spend !!!");
    }
}