using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Categories.Commands
{
    public sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
    {
        private readonly ICategoryWriteRepository _categoryWriteRepository;
        private readonly ICategoryReadRepository _categoryReadRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategoryCommandHandler(
            ICategoryWriteRepository categoryWriteRepository,
            ICategoryReadRepository categoryReadRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _categoryWriteRepository = categoryWriteRepository;
            _categoryReadRepository = categoryReadRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (_currentUserService.Role != Role.Admin)
                throw new DomainUnauthorizedException("Only admin can create category");

            if (await _categoryReadRepository.IsCategoryNameExistAsync(request.CategoryName, cancellationToken))
                throw new DomainRuleException("Category name already existed.");

            var category = Category.Create(request.CategoryName, request.Descriptions);
            _categoryWriteRepository.Add(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return category.Id;
        }
    }
}
