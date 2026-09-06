using Application.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Categories.Commands
{
    public sealed class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
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

        public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {

            var category = await _categoryWriteRepository.GetCategoryByIdAsync(request.CategoryId, cancellationToken) ??
                throw new DomainNotFoundException("Category not found");

            _categoryWriteRepository.Remove(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _cacheService.RemoveCacheResource($"category:{request.CategoryId}");
        }
    }
}
