using MediatR;
using Models.Dtos;

namespace BuyActions.Queries;

public record GetUserQuery(Guid UserId) : IRequest<UserDto?>;