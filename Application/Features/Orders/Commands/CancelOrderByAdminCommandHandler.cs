using Application.Interfaces;
using Domain.Exceptions;
using MediatR;
namespace Application.Features.Orders.Commands
{
    public sealed class CancelOrderByAdminCommandHandler : IRequestHandler<CancelOrderByAdminCommand>
    {
        private readonly IOrderWriteRepository _orderWriteRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CancelOrderByAdminCommandHandler(
            IOrderWriteRepository orderWriteRepository,
            IProductWriteRepository productWriteRepository,
            IUnitOfWork unitOfWork)
        {
            _orderWriteRepository = orderWriteRepository;
            _productWriteRepository = productWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(CancelOrderByAdminCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderWriteRepository.GetOrderByIdAsync(request.OrderId, cancellationToken) ??
                throw new DomainNotFoundException("Order not found.");

            var isOrderCancel = order.CancelOrder();
            if (!isOrderCancel) return;

            foreach (var item in order.OrderItems)
            {
                var product = await _productWriteRepository.GetProductByIdAsync(item.ProductId, cancellationToken) ??
                   throw new DomainNotFoundException("Product not found");

                product.IncreaseStock(item.Quantity);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
