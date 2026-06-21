using FluentValidation;
using User.Application.Dto;
using User.Application.Interfaces;
using User.Application.Mappings;
using UnauthorizedAccessException = User.Application.Exceptions.UnauthorizedAccessException;

namespace User.Application.Services;

public class UserService(IUserRepository repository, IJwtTokenGenerator jwtTokenGenerator, 
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
        if(!Enum.TryParse<Model.Role>(userDto.Role, out var role)) throw new ArgumentException("Invalid role");
        var user = Model.User.CreateUser(
            userDto.Name,
            userDto.Email,
            userDto.Password,
            userDto.Wallet,
            role);
        var createdUser = await repository.AddAsync(user);
        return createdUser.ToDto();
    }

    public async Task Delete(Guid userId)
    {
        await repository.DeleteAsync(userId);
    }

    public async Task<string> Authorize(UserCredentialsDto credentials)
    {
        var user = await repository.Authorize(credentials);
        if (user == null) throw new UnauthorizedAccessException("Invalid credentials");
        return jwtTokenGenerator.GenerateJwtToken(user.Id, user.Role);
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