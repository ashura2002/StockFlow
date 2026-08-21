using Application.Dtos;
using Application.Interfaces;
using Domain.Enums;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Categories.Queries
{
    public sealed class GetAllCategoriesQueryhandler : IRequestHandler<GetAllCategoriesQuery, IReadOnlyCollection<CategoryResponseDto>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ICategoryReadRepository _categoryReadRepository;

        public GetAllCategoriesQueryhandler(
            ICurrentUserService currentUserService,
            ICategoryReadRepository categoryReadRepository)
        {
            _currentUserService = currentUserService;
            _categoryReadRepository = categoryReadRepository;
        }

        public async Task<IReadOnlyCollection<CategoryResponseDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            if (_currentUserService.Role != Role.Admin)
                throw new DomainUnauthorizedException("Only admin can access this resources.");

            return await _categoryReadRepository.GetAllCategoriesAsync(cancellationToken);
        }
    }
}
