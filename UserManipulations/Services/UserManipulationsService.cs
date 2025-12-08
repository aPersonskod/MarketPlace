using Microsoft.EntityFrameworkCore;
using Models;
using Models.Interfaces;

namespace UserManipulations.Services;

public class UserManipulationsService(DataContext dataContext) : IUserManipulations
{
    public Task<IEnumerable<User>> Get() => Task.FromResult<IEnumerable<User>>(dataContext.Users);

    public async Task<User> Get(Guid userId)
    {
        var user = await dataContext.Users.FindAsync(userId);
        if (user != null) return await Task.FromResult(user);
        throw new Exception("User not found");
    }

    public async Task<User> Authorize(string email, string password)
    {
        var user = await dataContext.Users.FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
        if (user != null) return await Task.FromResult(user);
        throw new Exception("User not found");
    }

    public async Task<User> Add(User user)
    {
        if(await dataContext.Users.AnyAsync(x => x.Email == user.Email))
            throw new Exception("User already exists");
        var newUser = new User()
        {
            Id = Guid.NewGuid(),
            Name = user.Name,
            Email = user.Email,
            Password = user.Password,
        };
        dataContext.Users.Add(newUser);
        await dataContext.SaveChangesAsync();
        return await Task.FromResult(newUser);
    }
    
    public async Task<User> Update(User user)
    {
        var foundUser = await dataContext.Users.FindAsync(user.Id);
        if (foundUser == null) throw new Exception("User not found");
        foundUser.Name = user.Name;
        foundUser.Email = user.Email;
        foundUser.Password = user.Password;
        await dataContext.SaveChangesAsync();
        return await Task.FromResult(foundUser);
    }

    public async Task Delete(Guid userId)
    {
        var foundUser = await dataContext.Users.FindAsync(userId);
        if (foundUser == null) throw new Exception("User not found");
        dataContext.Users.Remove(foundUser);
        await dataContext.SaveChangesAsync();
    }

    public async Task<User> WalletReplenishment(Guid userId, int money)
    {
        var foundUser = await dataContext.Users.FindAsync(userId);
        if (foundUser == null) throw new Exception("User not found");
        foundUser.Wallet += money;
        await dataContext.SaveChangesAsync();
        return await Task.FromResult(foundUser);
    }

    public async Task<User> SpendMoney(Guid userId, int money)
    {
        var foundUser = await dataContext.Users.FindAsync(userId);
        if (foundUser == null) throw new Exception("User not found");
        if (foundUser.Wallet >= money)
        {
            foundUser.Wallet -= money;
            await dataContext.SaveChangesAsync();
            return await Task.FromResult(foundUser);
        }
        throw new Exception("User has not enough money to spend !!!");
    }
}