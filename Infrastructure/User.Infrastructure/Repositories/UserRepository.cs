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
    public async Task<Model.User> AddAsync(Model.User entity)
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