using MediatR;
using Models.Dtos;

namespace BuyActions.Queries;

public record GetCartQuery(Guid CartId) : IRequest<CartDto?>;