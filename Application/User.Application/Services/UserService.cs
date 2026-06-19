using Models;
using User.Application.Dto;
using User.Application.Interfaces;

namespace User.Application.Services;

public class UserService(IUserRepository repository) : IUserService
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
        if(!Enum.TryParse<Role>(userDto.Role, out var role)) throw new Exception("Invalid role");
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

    public async Task<UserDto?> Authorize(UserCredentialsDto credentials)
    {
        var user = await repository.Authorize(credentials);
        return GetUserDto(user);
    }

    public async Task<UserDto> TopUpMoney(UserMoneyDto userMoneyDto)
    {
        var user = await repository.WalletReplenishment(userMoneyDto);
        return GetUserDto(user);
    }

    public async Task<UserDto> SpendMoney(UserMoneyDto userMoneyDto)
    {
        var user = await repository.SpendMoney(userMoneyDto);
        return GetUserDto(user);
    }
    
    private UserDto GetUserDto(Models.User? user)
    {
        if (user == null) throw new Exception("Convert to dto error: user not found");
        if (!Enum.TryParse<Models.Role>(user.Role, out var role)) throw new Exception("Convert to dto error: Invalid role");
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