using MediatR;
using Models.Dtos;

namespace BuyActions.Queries;

public record GetProductQuery(Guid ProductId) : IRequest<ProductDto?>;