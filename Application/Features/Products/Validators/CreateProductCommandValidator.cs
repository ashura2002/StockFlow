using Application.Features.Products.Commands;
using FluentValidation;

namespace Application.Features.Products.Validators
{
    public sealed class CreateProductCommandValidator
        : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(50);

            RuleFor(x => x.Price)
                .GreaterThan(0);

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.CategoryId)
                .NotEmpty();

            RuleFor(x => x.SupplierId)
                .NotEmpty();

            RuleFor(x => x.ProductDescriptions)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.ProductDescriptions));
        }
    }
}