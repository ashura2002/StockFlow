using Application.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Products.Commands
{
    public sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
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

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {

            var product = await _productWriteRepository.GetProductByIdAsync(request.ProductId, cancellationToken) ??
                throw new DomainNotFoundException("Product not found.");

            product.SoftDelete();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _cacheService.RemoveCacheResource($"product:{request.ProductId}");
        }
    }
}
