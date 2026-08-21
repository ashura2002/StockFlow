using Application.Dtos;
using MediatR;

namespace Application.Features.Categories.Queries
{
    public sealed record GetAllCategoriesQuery() : IRequest<IReadOnlyCollection<CategoryResponseDto>>;
}
