using FluentValidation;
using User.Application.Dto;

namespace User.Application.Validations;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required").
            MaximumLength(100).WithMessage("Name must be between 3 and 100 characters");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email valid address is required");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required")
            .MinimumLength(5).WithMessage("Password must be between 6 and 20 characters")
            .MaximumLength(20).WithMessage("Password must be between 6 and 20 characters");
        RuleFor(x => x.Wallet).NotEmpty().WithMessage("Wallet is required")
            .GreaterThan(0).WithMessage("Wallet must be greater than 0");
        RuleFor(x => x.Role).NotEmpty().WithMessage("Role is required")
            .Must(x => Enum.TryParse<Models.Role>(x, true, out _)).WithMessage("The specified role name does not exist");
    }
}