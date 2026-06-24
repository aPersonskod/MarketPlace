using BuyReport.Application.Features.Commands;
using FluentValidation;

namespace BuyReport.Application.Validation;

public class CreateBuyReportCommandValidator : AbstractValidator<CreateBuyReportCommand>
{
    public CreateBuyReportCommandValidator()
    {
        RuleFor(x => x.CartId).NotEmpty().WithMessage("CartId is required.");
    }
}