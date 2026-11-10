using Application.Common.Results;
using Application.Dtos;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Categories.Queries
{
    public sealed class GetAllCategoriesQueryhandler : IRequestHandler<GetAllCategoriesQuery, ResultWithValue<IReadOnlyCollection<CategoryResponseDto>>>
    {
        private readonly ICategoryReadRepository _categoryReadRepository;

        public GetAllCategoriesQueryhandler(
            ICategoryReadRepository categoryReadRepository)
        {
            _categoryReadRepository = categoryReadRepository;
        }

        public async Task<ResultWithValue<IReadOnlyCollection<CategoryResponseDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _categoryReadRepository.GetAllCategoriesAsync(cancellationToken);
            return ResultWithValue<IReadOnlyCollection<CategoryResponseDto>>.Success(categories);
        }
    }
}
