using Application.Common.Errors;
using Application.Common.Results;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Categories.Commands
{
    public sealed class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result>
    {
        private readonly ICategoryWriteRepository _categoryWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;

        public DeleteCategoryCommandHandler(
            ICategoryWriteRepository categoryWriteRepository,
            IUnitOfWork unitOfWork,
            ICacheService cacheService)
        {
            _categoryWriteRepository = categoryWriteRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {

            var category = await _categoryWriteRepository.GetCategoryByIdAsync(request.CategoryId, cancellationToken);
            if (category is null)
                return Result.Failure(CategoryErrors.CategoryNotFound);

            _categoryWriteRepository.Remove(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _cacheService.RemoveCacheResource($"category:{request.CategoryId}");
            return Result.Success();
        }
    }
}
