using User.Application.Dto;
using User.Application.Exceptions;

namespace User.Application.Mappings;

public static class MappingExtensions
{
    public static UserDto ToDto(this Model.User? user)
    {
        if (user == null) throw new NotFoundException("User not found");
        if (!Enum.TryParse<Model.Role>(user.Role, out var role)) throw new ArgumentException("Invalid role");
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