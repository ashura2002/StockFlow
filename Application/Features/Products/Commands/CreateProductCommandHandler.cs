using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Products.Commands
{
    public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly ICategoryReadRepository _categoryReadRepository;
        private readonly ISupplierReadRepository _supplierReadRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(
            ICurrentUserService currentUserService,
            IProductWriteRepository productWriteRepository,
            IProductReadRepository productReadRepository,
            ICategoryReadRepository categoryReadRepository,
            ISupplierReadRepository supplierReadRepository,
            IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _categoryReadRepository = categoryReadRepository;
            _supplierReadRepository = supplierReadRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            if (_currentUserService.Role != Role.Admin)
                throw new DomainUnauthorizedException("Only admin can create product.");

            if (!await _categoryReadRepository.IsCategoryExistAsync(request.CategoryId, cancellationToken))
                throw new DomainNotFoundException("Category not found.");

            if (!await _supplierReadRepository.IsSupplierExistAsync(request.SupplierId, cancellationToken))
                throw new DomainNotFoundException("Supplier not found.");

            if (await _productReadRepository.IsProductNameExistAsync(request.ProductName, cancellationToken))
                throw new DomainRuleException("Product name is already existed.");

            var productName = ProductNameVo.Create(request.ProductName);

            var product = Product.Create(
                productName, 
                request.Price, 
                request.Stock, 
                request.CategoryId, 
                request.SupplierId,
                request.ProductDescriptions);

            _productWriteRepository.Add(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}
