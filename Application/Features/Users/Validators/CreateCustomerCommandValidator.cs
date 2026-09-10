using Application.Features.Users.Commands;
using FluentValidation;

namespace Application.Features.Users.Validators
{
    public sealed class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(user => user.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(user => user.Password)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(50);

            RuleFor(user => user.ConfirmPassword)
                .Equal(user => user.Password)
                .WithMessage("Password does not match.");
        }
    }
}
