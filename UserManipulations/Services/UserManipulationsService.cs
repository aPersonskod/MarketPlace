using Models;
using Models.Interfaces;

namespace UserManipulations.Services;

public class UserManipulationsService(DataContext dataContext) : IUserManipulations
{
    public Task<IEnumerable<User>> Get() => Task.FromResult<IEnumerable<User>>(dataContext.Users);

    public Task<User> Get(Guid userId)
    {
        var user = dataContext.Users.FirstOrDefault(x => x.Id == userId);
        if (user != null) return Task.FromResult(user);
        throw new Exception("User not found");
    }

    public Task<User> Authorize(string email, string password)
    {
        var user = dataContext.Users.FirstOrDefault(x => x.Email == email && x.Password == password);
        if (user != null) return Task.FromResult(user);
        throw new Exception("User not found");
    }

    public Task<User> Add(User user)
    {
        if(dataContext.Users.Any(x => x.Email == user.Email))
            throw new Exception("User already exists");
        var newUser = new User()
        {
            Id = Guid.NewGuid(),
            Name = user.Name,
            Email = user.Email,
            Password = user.Password,
        };
        dataContext.Users.Add(newUser);
        return Task.FromResult(newUser);
    }
    
    public Task<User> Update(User user)
    {
        var foundUser = dataContext.Users.FirstOrDefault(x => x.Id == user.Id);
        if (foundUser == null) throw new Exception("User not found");
        foundUser.Name = user.Name;
        foundUser.Email = user.Email;
        foundUser.Password = user.Password;
        return Task.FromResult(foundUser);

    }

    public async Task Delete(Guid userId)
    {
        var foundUser = dataContext.Users.FirstOrDefault(x => x.Id == userId);
        if (foundUser == null) throw new Exception("User not found");
        dataContext.Users.Remove(foundUser);
    }

    public Task<User> WalletReplenishment(Guid userId, int money)
    {
        var foundUser = dataContext.Users.FirstOrDefault(x => x.Id == userId);
        if (foundUser == null) throw new Exception("User not found");
        foundUser.Wallet += money;
        return Task.FromResult(foundUser);
    }

    public Task<User> SpendMoney(Guid userId, int money)
    {
        var foundUser = dataContext.Users.FirstOrDefault(x => x.Id == userId);
        if (foundUser == null) throw new Exception("User not found");
        if (foundUser.Wallet >= money)
        {
            foundUser.Wallet -= money;
            return Task.FromResult(foundUser);
        }
        throw new Exception("User has not enough money to spend !!!");
    }
}