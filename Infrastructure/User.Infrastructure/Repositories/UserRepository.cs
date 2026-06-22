using Microsoft.EntityFrameworkCore;
using Model.SharedExceptions;
using User.Application.Dto;
using User.Application.Interfaces;
using User.Infrastructure.Data;

namespace User.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<IEnumerable<Model.User>> GetAllAsync() => await context.Users.ToListAsync();
    public async Task<Model.User?> GetByIdAsync(Guid id) => await context.Users.FirstOrDefaultAsync(x => x.Id == id);
    public async Task<Model.User> AddAsync(CreateUserDto userDto)
    {
        if(!Enum.TryParse<Model.Role>(userDto.Role, out var role)) throw new ArgumentException("Invalid role");
        var user = Model.User.CreateUser(
            userDto.Name,
            userDto.Email,
            userDto.Password,
            userDto.Wallet,
            role);
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return user;
    }
    public async Task DeleteAsync(Guid id)
    {
        var foundItem = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (foundItem == null) throw new NotFoundException("User not found");
        context.Users.Remove(foundItem);
        await context.SaveChangesAsync();
    }
    public async Task<Model.User?> Authorize(UserCredentialsDto credentials) 
        => await context.Users.FirstOrDefaultAsync(x => x.Email == credentials.Email && x.Password == credentials.Password);
    public async Task<Model.User> WalletReplenishment(UserMoneyDto userMoneyDto)
    {
        var foundItem = await context.Users.FirstOrDefaultAsync(x => x.Id == userMoneyDto.UserId);
        if (foundItem == null) throw new NotFoundException("User not found");
        foundItem.AddMoney(userMoneyDto.Money);
        await context.SaveChangesAsync();
        return foundItem;
    }
    public async Task<Model.User> SpendMoney(UserMoneyDto userMoneyDto)
    {
        var foundItem = await context.Users.FirstOrDefaultAsync(x => x.Id == userMoneyDto.UserId);
        if (foundItem == null) throw new NotFoundException("User not found");
        foundItem.SpendMoney(userMoneyDto.Money);
        await context.SaveChangesAsync();
        return foundItem;
    }
}