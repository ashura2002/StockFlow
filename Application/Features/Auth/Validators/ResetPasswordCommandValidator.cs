using Application.Features.Auth.Commands;
using FluentValidation;

namespace Application.Features.Auth.Validators
{
    public sealed class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.RawToken)
                .NotEmpty();

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(50);
        }
    }
}
