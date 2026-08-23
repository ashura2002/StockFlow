using Application.Interfaces;
using Domain.Enums;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Products.Commands
{
    public sealed class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(
            ICurrentUserService currentUserService,
            IProductWriteRepository productWriteRepository,
            IUnitOfWork  unitOfWork)
        {
            _currentUserService = currentUserService;
            _productWriteRepository = productWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            if (_currentUserService.Role != Role.Admin)
                throw new DomainUnauthorizedException("Only admin can delete this resources");

            var product = await _productWriteRepository.GetProductByIdAsync(request.ProductId, cancellationToken)??
                throw new DomainNotFoundException("Product not found.");
            product.SoftDelete();

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
