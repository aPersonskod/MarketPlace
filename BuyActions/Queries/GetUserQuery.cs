using MediatR;
using Models.Dtos;

namespace BuyActions.Queries;

public record GetUserQuery(string AccessToken) : IRequest<UserDto?>;