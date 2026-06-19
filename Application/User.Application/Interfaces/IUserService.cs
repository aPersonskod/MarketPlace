using User.Application.Dto;

namespace User.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> Get();
    Task<UserDto> Get(Guid userId);
    Task<UserDto> Add(CreateUserDto userDto);
    //Task<UserDto> Update(UserDto userDto);
    Task Delete(Guid userId);
    Task<UserDto?> Authorize(UserCredentialsDto credentials);
    Task<UserDto> TopUpMoney(UserMoneyDto userMoneyDto);
    Task<UserDto> SpendMoney(UserMoneyDto userMoneyDto);
}