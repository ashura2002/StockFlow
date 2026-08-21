using Application.Interfaces;
using Domain.Enums;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Categories.Commands
{
    public sealed class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ICategoryWriteRepository _categoryWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryCommandHandler(
            ICurrentUserService currentUserService,
            ICategoryWriteRepository categoryWriteRepository,
            IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _categoryWriteRepository = categoryWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            if (_currentUserService.Role != Role.Admin)
                throw new DomainRuleException("Only admin can delete category");

            var category = await _categoryWriteRepository.GetCategoryByIdAsync(request.CategoryId, cancellationToken) ??
                throw new DomainNotFoundException("Category not found");

            _categoryWriteRepository.Remove(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
