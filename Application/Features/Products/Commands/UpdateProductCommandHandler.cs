using Application.Interfaces;
using Domain.Enums;
using Domain.Exceptions;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Products.Commands
{
    public sealed class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(
            IProductWriteRepository productWriteRepository,
            IProductReadRepository productReadRepository,
            IUnitOfWork unitOfWork)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {

            var productName = ProductNameVo.Create(request.ProductName);

            var product = await _productWriteRepository.GetProductByIdAsync(request.ProductId, cancellationToken) ??
                throw new DomainNotFoundException("Product not found");

            if (await _productReadRepository.IsProductNameExistAsync(productName.Value, product.Id, cancellationToken))
                throw new DomainConflictException("Product name already exits.");

            product.UpdateProductName(productName);
            product.UpdatePrice(request.Price);
            product.UpdateProductDescriptions(request.Descriptions);
            product.UpdateProductStock(request.Stock);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
