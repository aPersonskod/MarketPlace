using MediatR;
using Models.Dtos;

namespace BuyActions.Commands;

public record BuyCartCommand(CartDto CartDto, string AccessToken) : IRequest<bool>;