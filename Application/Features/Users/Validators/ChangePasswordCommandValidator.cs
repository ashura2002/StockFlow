using Application.Features.Users.Commands;
using FluentValidation;

namespace Application.Features.Users.Validators
{
    public sealed class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {

        public ChangePasswordCommandValidator()
        {
            RuleFor(user => user.CurrentPassword)
                .NotEmpty();

            RuleFor(user => user.NewPassword)
                  .NotEmpty()
                  .MinimumLength(8)
                  .MaximumLength(50);

            RuleFor(user => user.ConfirmNewPassword)
                .Equal(user => user.NewPassword)
                .WithMessage("Password does not match.");
        }
    }
}
