using Models.Dtos;

namespace Models.Interfaces;

public interface IUserManipulations
{
    Task<IEnumerable<UserDto>> Get();
    Task<UserDto> Get(Guid userId);
    Task<UserDto> Authorize(string email, string password);
    Task<UserDto> Add(UserDto userDto);
    Task<UserDto> Update(UserDto userDto);
    Task Delete(Guid userId);
    Task<UserDto> WalletReplenishment(Guid userId, int money);
    Task<UserDto> SpendMoney(Guid userId, int money);
}