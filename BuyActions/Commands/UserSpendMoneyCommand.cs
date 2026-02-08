using MediatR;
using Models.Dtos;

namespace BuyActions.Commands;

public record UserSpendMoneyCommand(Guid UserId, int Money) : IRequest<UserDto?>;