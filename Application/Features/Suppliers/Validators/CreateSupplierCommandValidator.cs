using Application.Features.Suppliers.Commands;
using FluentValidation;

namespace Application.Features.Suppliers.Validators
{
    public sealed class CreateSupplierCommandValidator
        : AbstractValidator<CreateSupplierCommand>
    {
        public CreateSupplierCommandValidator()
        {
            RuleFor(x => x.SupplierName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches(@"^09\d{9}$")
                .WithMessage("Phone number must be a valid Philippine mobile number.");

            RuleFor(x => x.Address)
                .NotEmpty()
                .MaximumLength(255);
        }
    }
}