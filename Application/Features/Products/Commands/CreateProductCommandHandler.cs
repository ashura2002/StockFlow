using Application.Common.Errors;
using Application.Common.Results;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Products.Commands
{
    public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ResultWithValue<Guid>>
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly ICategoryReadRepository _categoryReadRepository;
        private readonly ISupplierReadRepository _supplierReadRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(
            IProductWriteRepository productWriteRepository,
            IProductReadRepository productReadRepository,
            ICategoryReadRepository categoryReadRepository,
            ISupplierReadRepository supplierReadRepository,
            IUnitOfWork unitOfWork)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _categoryReadRepository = categoryReadRepository;
            _supplierReadRepository = supplierReadRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultWithValue<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var productName = ProductNameVo.Create(request.ProductName);

            var isCategoryExist = await _categoryReadRepository.IsCategoryExistAsync(request.CategoryId, cancellationToken);
            if (!isCategoryExist)
                return ResultWithValue<Guid>.Failure(CategoryErrors.CategoryNotFound);

            var isSupplierExist = await _supplierReadRepository.IsSupplierExistAsync(request.SupplierId, cancellationToken);
            if (!isSupplierExist)
                return ResultWithValue<Guid>.Failure(SupplierErrors.SupplierNotFound);

            var isProductNameExist = await _productReadRepository.IsProductNameExistAsync(productName.Value, null, cancellationToken);
            if (isProductNameExist)
                return ResultWithValue<Guid>.Failure(ProductErrors.ProductNameAlreadyExisted);


            var product = Product.Create(
                productName,
                request.Price,
                request.Stock,
                request.CategoryId,
                request.SupplierId,
                request.ProductDescriptions);

            _productWriteRepository.Add(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ResultWithValue<Guid>.Success(product.Id);
        }
    }
}
