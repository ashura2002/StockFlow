
using Application.Dtos;
using MediatR;

namespace Application.Features.Products.Queries
{
    public sealed record GetAllDeletedProductQuery : IRequest<IReadOnlyCollection<DeletedProductResponseDto>>;
}
