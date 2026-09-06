using Application.Dtos;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Categories.Queries
{
    public sealed class GetCategoryByIdWithProductsQueryHandler : IRequestHandler<GetCategoryByIdWithProductsQuery, CategoryWithProductsResponseDto>
    {
        private readonly ICategoryReadRepository _categoryReadRepository;
        private readonly ICacheService _cacheService;
        public GetCategoryByIdWithProductsQueryHandler(
            ICategoryReadRepository categoryReadRepository,
            ICacheService cacheService)
        {
            _categoryReadRepository = categoryReadRepository;
            _cacheService = cacheService;
        }

        public async Task<CategoryWithProductsResponseDto> Handle(GetCategoryByIdWithProductsQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"category:{request.CategoryId}";

            return await _cacheService.GetOrCreateAsync(
                cacheKey,
                () => _categoryReadRepository.GetCategoryByIdWithProductsAsync(request.CategoryId, cancellationToken),
                TimeSpan.FromMinutes(5)
                ) ??
                throw new DomainNotFoundException("Category not found.");
        }
    }
}
