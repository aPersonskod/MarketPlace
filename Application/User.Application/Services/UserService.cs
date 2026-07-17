using FluentValidation;
using User.Application.Dto;
using User.Application.Interfaces;
using User.Application.Mappings;

namespace User.Application.Services;

public class UserService(IUserRepository repository, 
    IValidator<CreateUserDto> createUserValidator, IValidator<UserMoneyDto> moneyDtoValidator) : IUserService
{
    public async Task<IEnumerable<UserDto>> Get()
    {
        var users = await repository.GetAllAsync();
        return users.Select(x => x.ToDto());
    }

    public async Task<UserDto> Get(Guid userId)
    {
        var user = await repository.GetByIdAsync(userId);
        return user.ToDto();
    }

    public async Task<UserDto> Add(CreateUserDto userDto)
    {
        await createUserValidator.ValidateAndThrowAsync(userDto);
        var createdUser = await repository.AddAsync(userDto);
        return createdUser.ToDto();
    }

    public async Task Delete(Guid userId)
    {
        await repository.DeleteAsync(userId);
    }

    public async Task<UserDto> TopUpMoney(UserMoneyDto userMoneyDto)
    {
        await moneyDtoValidator.ValidateAndThrowAsync(userMoneyDto);
        var user = await repository.WalletReplenishment(userMoneyDto);
        return user.ToDto();
    }

    public async Task<UserDto> SpendMoney(UserMoneyDto userMoneyDto)
    {
        await moneyDtoValidator.ValidateAndThrowAsync(userMoneyDto);
        var user = await repository.SpendMoney(userMoneyDto);
        return user.ToDto();
    }
}