using FluentValidation;
using User.Application.Dto;

namespace User.Application.Validations;

public class MoneyDtoValidator : AbstractValidator<UserMoneyDto>
{
    public MoneyDtoValidator()
    {
        RuleFor(x => x.Money).NotEmpty().WithMessage("Wallet is required")
            .GreaterThan(0).WithMessage("Wallet must be greater than 0");
    }
}