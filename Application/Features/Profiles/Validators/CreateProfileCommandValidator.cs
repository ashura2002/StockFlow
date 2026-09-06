using Application.Features.Profiles.Commands;
using FluentValidation;

namespace Application.Features.Profiles.Validators
{
    public sealed class CreateProfileCommandValidator
        : AbstractValidator<CreateProfileCommand>
    {
        public CreateProfileCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(50);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(50);

            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .LessThan(DateOnly.FromDateTime(DateTime.Today));

            RuleFor(x => x.Address)
                .NotEmpty()
                .MaximumLength(255);
        }
    }
}