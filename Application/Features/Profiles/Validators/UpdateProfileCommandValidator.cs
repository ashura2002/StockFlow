using Application.Features.Profiles.Commands;
using FluentValidation;

namespace Application.Features.Profiles.Validators
{
    public sealed class UpdateProfileCommandValidator
        : AbstractValidator<UpdateProfileCommand>
    {
        public UpdateProfileCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(50);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(50);

            RuleFor(x => x.Address)
                .NotEmpty()
                .MaximumLength(255);
        }
    }
}