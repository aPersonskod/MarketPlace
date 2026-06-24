using BuyReport.Application.Dtos;

namespace BuyReport.Application.Interfaces;

public interface IUserRepository
{
    Task<UserDto?> GetUserAsync(string? authToken);
}