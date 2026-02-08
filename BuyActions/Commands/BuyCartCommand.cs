using MediatR;
using Models.Dtos;

namespace BuyActions.Commands;

public record BuyCartCommand(CartDto CartDto) : IRequest<bool>;