using FluentValidation;
using Models;
using User.Application.Dto;
using User.Application.Exceptions;
using User.Application.Interfaces;
using User.Application.Validations;
using UnauthorizedAccessException = User.Application.Exceptions.UnauthorizedAccessException;

namespace User.Application.Services;

public class UserService(IUserRepository repository, IJwtTokenGenerator jwtTokenGenerator, 
    IValidator<CreateUserDto> createUserValidator, IValidator<UserMoneyDto> moneyDtoValidator) : IUserService
{
    public async Task<IEnumerable<UserDto>> Get()
    {
        var users = await repository.GetAllAsync();
        return users.Select(GetUserDto);
    }

    public async Task<UserDto> Get(Guid userId)
    {
        var user = await repository.GetByIdAsync(userId);
        return GetUserDto(user);
    }

    public async Task<UserDto> Add(CreateUserDto userDto)
    {
        await createUserValidator.ValidateAndThrowAsync(userDto);
        if(!Enum.TryParse<Role>(userDto.Role, out var role)) throw new ArgumentException("Invalid role");
        var user = Models.User.CreateUser(
            userDto.Name,
            userDto.Email,
            userDto.Password,
            userDto.Wallet,
            role);
        var createdUser = await repository.AddAsync(user);
        return GetUserDto(createdUser);
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
        return GetUserDto(user);
    }

    public async Task<UserDto> SpendMoney(UserMoneyDto userMoneyDto)
    {
        await moneyDtoValidator.ValidateAndThrowAsync(userMoneyDto);
        var user = await repository.SpendMoney(userMoneyDto);
        return GetUserDto(user);
    }
    
    private UserDto GetUserDto(Models.User? user)
    {
        if (user == null) throw new NotFoundException("User not found");
        if (!Enum.TryParse<Models.Role>(user.Role, out var role)) throw new ArgumentException("Invalid role");
        var dto = new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Wallet = user.Wallet,
            Role = role.ToString()
        };
        return dto;
    }
}