using Application.Dtos;
using MediatR;

namespace Application.Features.Categories.Queries
{
    public sealed record GetCategoryByIdWithProductsQuery(Guid CategoryId) : IRequest<CategoryWithProductsResponseDto>;
}
