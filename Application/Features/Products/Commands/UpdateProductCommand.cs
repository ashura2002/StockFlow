using MediatR;

namespace Application.Features.Products.Commands
{
    public record UpdateProductCommand(
        Guid ProductId,
        string ProductName,
        decimal Price,
        int Stock,
        string? Descriptions) : IRequest;
}
