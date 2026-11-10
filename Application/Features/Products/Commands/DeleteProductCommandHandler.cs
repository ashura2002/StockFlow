using Application.Common.Errors;
using Application.Common.Results;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Products.Commands
{
    public sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result>
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;

        public DeleteProductCommandHandler(
            IProductWriteRepository productWriteRepository,
            IUnitOfWork unitOfWork,
            ICacheService cacheService)
        {
            _productWriteRepository = productWriteRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {

            var product = await _productWriteRepository.GetProductByIdAsync(request.ProductId, cancellationToken);
            if (product is null)
                return Result.Failure(ProductErrors.ProductNotFound);

            product.SoftDelete();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _cacheService.RemoveCacheResource($"product:{request.ProductId}");

            return Result.Success();
        }
    }
}
