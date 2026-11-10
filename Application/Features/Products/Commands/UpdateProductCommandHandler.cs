using Application.Common.Errors;
using Application.Common.Results;
using Application.Interfaces;
using Domain.Exceptions;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Products.Commands
{
    public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result>
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;

        public UpdateProductCommandHandler(
            IProductWriteRepository productWriteRepository,
            IProductReadRepository productReadRepository,
            IUnitOfWork unitOfWork,
            ICacheService cacheService)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {

            var productName = ProductNameVo.Create(request.ProductName);

            var product = await _productWriteRepository.GetProductByIdAsync(request.ProductId, cancellationToken);
            if (product is null)
                return Result.Failure(ProductErrors.ProductNotFound);

            var isProductNameAlreadyExist = await _productReadRepository.IsProductNameExistAsync(productName.Value, product.Id, cancellationToken);
            if (isProductNameAlreadyExist)
                return Result.Failure(ProductErrors.ProductNameAlreadyExisted);

            product.UpdateProductName(productName);
            product.UpdatePrice(request.Price);
            product.UpdateProductDescriptions(request.Descriptions);
            product.UpdateProductStock(request.Stock);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _cacheService.RemoveCacheResource($"product:{request.ProductId}");

            return Result.Success();
        }
    }
}
