using Microsoft.EntityFrameworkCore;
using Models;
using Models.Dtos;
using Models.Interfaces;

namespace UserManipulations.Services;

public class UserManipulationsService(DataContext dataContext) : IUserManipulations
{
    public Task<IEnumerable<UserDto>> Get() => Task.FromResult<IEnumerable<UserDto>>(dataContext.Users);

    public async Task<UserDto> Get(Guid userId)
    {
        var userDto = await dataContext.Users.FindAsync(userId);
        if (userDto == null) throw new Exception("User not found");
        return await Task.FromResult(userDto);
    }

    public async Task<UserDto> Authorize(string email, string password)
    {
        var userDto = await dataContext.Users.FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
        if (userDto == null) throw new Exception("User not found");
        return await Task.FromResult(userDto);
    }

    public async Task<UserDto> Add(UserDto userDto)
    {
        if(await dataContext.Users.AnyAsync(x => x.Email == userDto.Email))
            throw new Exception("User already exists");
        var newUserDto = new UserDto()
        {
            Id = Guid.NewGuid(),
            Name = userDto.Name,
            Email = userDto.Email,
            Password = userDto.Password,
        };
        dataContext.Users.Add(newUserDto);
        await dataContext.SaveChangesAsync();
        return await Task.FromResult(newUserDto);
    }
    
    public async Task<UserDto> Update(UserDto userDto)
    {
        var foundUserDto = await dataContext.Users.FindAsync(userDto.Id);
        if (foundUserDto == null) throw new Exception("User not found");
        foundUserDto.Name = userDto.Name;
        foundUserDto.Email = userDto.Email;
        foundUserDto.Password = userDto.Password;
        await dataContext.SaveChangesAsync();
        return await Task.FromResult(foundUserDto);
    }

    public async Task Delete(Guid userId)
    {
        var foundUserDto = await dataContext.Users.FindAsync(userId);
        if (foundUserDto == null) throw new Exception("User not found");
        dataContext.Users.Remove(foundUserDto);
        await dataContext.SaveChangesAsync();
    }

    public async Task<UserDto> WalletReplenishment(Guid userId, int money)
    {
        var foundUserDto = await dataContext.Users.FindAsync(userId);
        if (foundUserDto == null) throw new Exception("User not found");
        foundUserDto.Wallet += money;
        await dataContext.SaveChangesAsync();
        return await Task.FromResult(foundUserDto);
    }

    public async Task<UserDto> SpendMoney(Guid userId, int money)
    {
        var foundUserDto = await dataContext.Users.FindAsync(userId);
        if (foundUserDto == null) throw new Exception("User not found");
        if (foundUserDto.Wallet >= money)
        {
            foundUserDto.Wallet -= money;
            await dataContext.SaveChangesAsync();
            return await Task.FromResult(foundUserDto);
        }
        throw new Exception("User has not enough money to spend !!!");
    }
}