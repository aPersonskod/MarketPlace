using Microsoft.EntityFrameworkCore;
using User.Application.Dto;
using User.Application.Interfaces;
using User.Infrastructure.Data;

namespace User.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<IEnumerable<Models.User>> GetAllAsync() => await context.Users.ToListAsync();

    public async Task<Models.User?> GetByIdAsync(Guid id) => await context.Users.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<Models.User> AddAsync(Models.User entity)
    {
        await context.Users.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Guid id)
    {
        var foundItem = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (foundItem == null) return;
        context.Users.Remove(foundItem);
        await context.SaveChangesAsync();
    }

    public async Task<Models.User?> Authorize(UserCredentialsDto credentials) 
        => await context.Users.FirstOrDefaultAsync(x => x.Email == credentials.Email && x.Password == credentials.Password);

    public async Task<Models.User> WalletReplenishment(UserMoneyDto userMoneyDto)
    {
        var foundItem = await context.Users.FirstOrDefaultAsync(x => x.Id == userMoneyDto.Id);
        if (foundItem == null) throw new Exception("User not found");
        foundItem.AddMoney(userMoneyDto.Money);
        await context.SaveChangesAsync();
        return foundItem;
    }

    public async Task<Models.User> SpendMoney(UserMoneyDto userMoneyDto)
    {
        var foundItem = await context.Users.FirstOrDefaultAsync(x => x.Id == userMoneyDto.Id);
        if (foundItem == null) throw new Exception("User not found");
        foundItem.SpendMoney(userMoneyDto.Money);
        await context.SaveChangesAsync();
        return foundItem;
    }
}