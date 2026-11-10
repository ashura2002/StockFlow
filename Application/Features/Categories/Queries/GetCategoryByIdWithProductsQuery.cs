using Application.Common.Results;
using Application.Dtos;
using MediatR;

namespace Application.Features.Categories.Queries
{
    public sealed record GetCategoryByIdWithProductsQuery(Guid CategoryId) : IRequest<ResultWithValue<CategoryWithProductsResponseDto>>;
}
