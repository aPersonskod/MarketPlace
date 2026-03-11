using MediatR;
using Models.Dtos;

namespace BuyActions.Commands;

public record UserSpendMoneyCommand(int Money, string AccessToken) : IRequest<UserDto?>;