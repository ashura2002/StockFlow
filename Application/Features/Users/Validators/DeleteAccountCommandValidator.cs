

using Application.Features.Users.Commands;
using FluentValidation;

namespace Application.Features.Users.Validators
{
    public sealed class DeleteAccountCommandValidator : AbstractValidator<DeleteOwnAccountCommand>
    {
        public DeleteAccountCommandValidator()
        {
            RuleFor(User => User.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
