using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Categories.Commands
{
    public sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
    {
        private readonly ICategoryWriteRepository _categoryWriteRepository;
        private readonly ICategoryReadRepository _categoryReadRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategoryCommandHandler(
            ICategoryWriteRepository categoryWriteRepository,
            ICategoryReadRepository categoryReadRepository,
            IUnitOfWork unitOfWork)
        {
            _categoryWriteRepository = categoryWriteRepository;
            _categoryReadRepository = categoryReadRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryNameVo = CategoryNameVo.Create(request.CategoryName);

            if (await _categoryReadRepository.IsCategoryNameExistAsync(
                request.CategoryName, 
                null, 
                cancellationToken))
                throw new DomainConflictException("Category name already existed.");

            var category = Category.Create(categoryNameVo, request.Descriptions);
            _categoryWriteRepository.Add(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return category.Id;
        }
    }
}
