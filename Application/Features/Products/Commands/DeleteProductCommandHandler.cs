using Application.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Products.Commands
{
    public sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(
            IProductWriteRepository productWriteRepository,
            IUnitOfWork  unitOfWork)
        {
            _productWriteRepository = productWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {

            var product = await _productWriteRepository.GetProductByIdAsync(request.ProductId, cancellationToken)??
                throw new DomainNotFoundException("Product not found.");

            product.SoftDelete();

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
