using Model.SharedExceptions;
using Orchestrator.Application.Dtos;

namespace Orchestrator.Application.Mapping;

public static class MappingsExtensions
{
    public static CartDto ToDto(this Model.Cart? cart)
    {
        if (cart == null)
            throw new NotFoundException("Cart not found");
        return new CartDto()
        {
            Id = cart.Id,
            UserId = cart.UserId,
            PlaceId = cart.PlaceId,
            AmountToPay = cart.AmountToPay,
            IsConfirmed = cart.IsConfirmed,
            IsBought = cart.IsBought
        };
    }

    public static UserDto ToDto(this Model.User? user)
    {
        if (user == null)
            throw new NotFoundException("User not found");
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