using Application.Dtos;
using MediatR;

namespace Application.Features.Products.Queries
{
    public sealed record GetProductByIdQuery(Guid ProductId) : IRequest<ProductResponseDto>;
}
