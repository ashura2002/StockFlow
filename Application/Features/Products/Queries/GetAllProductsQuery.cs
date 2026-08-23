
using Application.Dtos;
using MediatR;

namespace Application.Features.Products.Queries
{
    public sealed record GetAllProductsQuery(int Page, int PageSize) : IRequest<IReadOnlyCollection<ProductResponseDto>>;
}
