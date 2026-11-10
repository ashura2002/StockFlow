using Application.Common.Errors;
using Application.Common.Results;
using Application.Dtos;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Categories.Queries
{
    public sealed class GetCategoryByIdWithProductsQueryHandler : IRequestHandler<GetCategoryByIdWithProductsQuery, ResultWithValue<CategoryWithProductsResponseDto>>
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

        public async Task<ResultWithValue<CategoryWithProductsResponseDto>> Handle(GetCategoryByIdWithProductsQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"category:{request.CategoryId}";

            var cachedCategory = _cacheService.GetCachedResource<CategoryWithProductsResponseDto>(cacheKey);

            if (cachedCategory is not null)
                return ResultWithValue<CategoryWithProductsResponseDto>.Success(cachedCategory);

            var category = await _categoryReadRepository.GetCategoryByIdWithProductsAsync(request.CategoryId, cancellationToken);

            if (category is null)
                return ResultWithValue<CategoryWithProductsResponseDto>.Failure(CategoryErrors.CategoryNotFound);

            _cacheService.Set(cacheKey, category, TimeSpan.FromMinutes(5));

            return ResultWithValue<CategoryWithProductsResponseDto>.Success(category);

        }
    }
}
