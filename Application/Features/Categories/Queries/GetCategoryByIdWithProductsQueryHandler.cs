using Application.Dtos;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Categories.Queries
{
    public sealed class GetCategoryByIdWithProductsQueryHandler : IRequestHandler<GetCategoryByIdWithProductsQuery, CategoryWithProductsResponseDto>
    {
        private readonly ICategoryReadRepository _categoryReadRepository;
        public GetCategoryByIdWithProductsQueryHandler(ICategoryReadRepository categoryReadRepository)
        {
            _categoryReadRepository = categoryReadRepository;
        }

        public async Task<CategoryWithProductsResponseDto> Handle(GetCategoryByIdWithProductsQuery request, CancellationToken cancellationToken)
        {
            return await _categoryReadRepository.GetCategoryByIdWithProductsAsync(
                request.CategoryId, 
                cancellationToken)??
                throw new DomainNotFoundException("Category not found.");
        }
    }
}
