using Cart.Application.Dtos;
using FluentValidation;

namespace Cart.Application.Validation;

public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
{
    public CreateOrderDtoValidator()
    {
        RuleFor(x => x.CartId).NotEmpty().WithMessage("CartId is required.");
        RuleFor(x => x.OrderedProductId).NotEmpty().WithMessage("ProductId is required.");
        RuleFor(x => x.Quantity).NotEmpty().WithMessage("Quantity is required")
            .GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}