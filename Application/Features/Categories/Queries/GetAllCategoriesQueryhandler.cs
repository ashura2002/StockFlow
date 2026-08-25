using Application.Dtos;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Categories.Queries
{
    public sealed class GetAllCategoriesQueryhandler : IRequestHandler<GetAllCategoriesQuery, IReadOnlyCollection<CategoryResponseDto>>
    {
        private readonly ICategoryReadRepository _categoryReadRepository;

        public GetAllCategoriesQueryhandler(
            ICategoryReadRepository categoryReadRepository)
        {
            _categoryReadRepository = categoryReadRepository;
        }

        public async Task<IReadOnlyCollection<CategoryResponseDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _categoryReadRepository.GetAllCategoriesAsync(cancellationToken);
        }
    }
}
