using Cart.Application.Dtos;
using FluentValidation;

namespace Cart.Application.Validation;

public class DeleteOrderDtoValidator : AbstractValidator<DeleteOrderDto>
{
    public DeleteOrderDtoValidator()
    {
        RuleFor(x => x.CartId).NotEmpty().WithMessage("CartId is required.");
        RuleFor(x => x.OrderedProductId).NotEmpty().WithMessage("ProductId is required.");
    }
}