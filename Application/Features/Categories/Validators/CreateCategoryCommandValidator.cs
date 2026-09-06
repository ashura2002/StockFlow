using Application.Features.Categories.Commands;
using FluentValidation;

namespace Application.Features.Categories.Validators
{
    public sealed class CreateCategoryCommandValidator
        : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.CategoryName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(50);

            RuleFor(x => x.Descriptions)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.Descriptions));
        }
    }
}