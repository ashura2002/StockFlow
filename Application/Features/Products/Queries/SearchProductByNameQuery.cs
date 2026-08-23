using Application.Dtos;
using MediatR;

namespace Application.Features.Products.Queries
{
    public sealed record SearchProductByNameQuery(
        string ProductName,
        int Page,
        int PageSize) : IRequest<IReadOnlyCollection<ProductResponseDto>>;
}
