using Microsoft.EntityFrameworkCore;
using Models;
using Models.Dtos;
using Models.Interfaces;

namespace UserManipulations.Services;

public class UserManipulationsService(DataContext dataContext) : IUserManipulations
{
    public Task<IEnumerable<UserDto>> Get() => Task.FromResult(dataContext.Users.Select(GetUserDto));

    public async Task<UserDto> Get(Guid userId)
    {
        var user = await dataContext.Users.FindAsync(userId);
        if (user == null) throw new Exception("User not found");
        return await Task.FromResult(GetUserDto(user));
    }

    public async Task<UserDto> Authorize(string email, string password)
    {
        var user = await dataContext.Users.FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
        if (user == null) throw new Exception("User not found");
        return await Task.FromResult(GetUserDto(user));
    }

    public async Task<UserDto> Add(UserDto userDto)
    {
        if(await dataContext.Users.AnyAsync(x => x.Email == userDto.Email))
            throw new Exception("User already exists");
        var newUser = new User()
        {
            Id = Guid.NewGuid(),
            Name = userDto.Name,
            Email = userDto.Email,
        };
        dataContext.Users.Add(newUser);
        await dataContext.SaveChangesAsync();
        return await Task.FromResult(GetUserDto(newUser));
    }
    
    public async Task<UserDto> Update(UserDto userDto)
    {
        var foundUser = await dataContext.Users.FindAsync(userDto.Id);
        if (foundUser == null) throw new Exception("User not found");
        foundUser.Name = userDto.Name;
        foundUser.Email = userDto.Email;
        await dataContext.SaveChangesAsync();
        return await Task.FromResult(GetUserDto(foundUser));
    }

    public async Task Delete(Guid userId)
    {
        var foundUser = await dataContext.Users.FindAsync(userId);
        if (foundUser == null) throw new Exception("User not found");
        dataContext.Users.Remove(foundUser);
        await dataContext.SaveChangesAsync();
    }

    public async Task<UserDto> WalletReplenishment(Guid userId, int money)
    {
        var foundUser = await dataContext.Users.FindAsync(userId);
        if (foundUser == null) throw new Exception("User not found");
        foundUser.Wallet += money;
        await dataContext.SaveChangesAsync();
        return await Task.FromResult(GetUserDto(foundUser));
    }

    public async Task<UserDto> SpendMoney(Guid userId, int money)
    {
        var foundUser = await dataContext.Users.FindAsync(userId);
        if (foundUser == null) throw new Exception("User not found");
        if (foundUser.Wallet >= money)
        {
            foundUser.Wallet -= money;
            await dataContext.SaveChangesAsync();
            return await Task.FromResult(GetUserDto(foundUser));
        }
        throw new Exception("User has not enough money to spend !!!");
    }

    private UserDto GetUserDto(User user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email,
        Wallet = user.Wallet
    };
}