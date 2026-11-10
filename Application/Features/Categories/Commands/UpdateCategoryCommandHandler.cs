using Application.Common.Errors;
using Application.Common.Results;
using Application.Interfaces;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Categories.Commands
{
    public sealed class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result>
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
        public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryNameVo = CategoryNameVo.Create(request.CategoryName);

            var category = await _categoryWriteRepository.GetCategoryByIdAsync(request.CategoryId, cancellationToken);
            if(category is null)
            return Result.Failure(CategoryErrors.CategoryNotFound);

            if (await _categoryReadRepository.IsCategoryNameExistAsync(categoryNameVo.Value, category.Id, cancellationToken))
                return Result.Failure(CategoryErrors.CategoryNameExisted);

            category.UpdateCategoryName(categoryNameVo);
            category.UpdateDescription(request.Description);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _cacheService.RemoveCacheResource($"category:{request.CategoryId}");

            return Result.Success();
        }
    }
}
