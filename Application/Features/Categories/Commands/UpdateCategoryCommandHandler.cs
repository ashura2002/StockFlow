using Application.Interfaces;
using Domain.Exceptions;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Categories.Commands
{
    public sealed class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly ICategoryWriteRepository _categoryWriteRepository;
        private readonly ICategoryReadRepository _categoryReadRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;

        public UpdateCategoryCommandHandler(
            ICategoryWriteRepository categoryWriteRepository,
            ICategoryReadRepository categoryReadRepository,
            IUnitOfWork unitOfWork,
            ICacheService cacheService)
        {
            _categoryWriteRepository = categoryWriteRepository;
            _categoryReadRepository = categoryReadRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }
        public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryNameVo = CategoryNameVo.Create(request.CategoryName);
            var category = await _categoryWriteRepository.GetCategoryByIdAsync(request.CategoryId, cancellationToken) ??
                throw new DomainNotFoundException("Category not found");
            if (await _categoryReadRepository.IsCategoryNameExistAsync(categoryNameVo.Value, category.Id, cancellationToken))
                throw new DomainConflictException("Category name is already existed.");

            category.UpdateCategoryName(categoryNameVo);
            category.UpdateDescription(request.Description);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _cacheService.RemoveCacheResource($"category:{request.CategoryId}");
        }
    }
}
