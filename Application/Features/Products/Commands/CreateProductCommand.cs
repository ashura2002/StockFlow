using MediatR;

namespace Application.Features.Products.Commands
{
    public sealed record CreateProductCommand(
        string ProductName,
        decimal Price,
        int Stock,
        Guid CategoryId,
        Guid SupplierId,
        string? ProductDescriptions) : IRequest<Guid>;
}
