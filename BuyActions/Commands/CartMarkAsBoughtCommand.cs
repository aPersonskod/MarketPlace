using MediatR;

namespace BuyActions.Commands;

public record CartMarkAsBoughtCommand(Guid CartId) : IRequest<bool>;