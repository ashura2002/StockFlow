using Application.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Orders.Commands
{
    public sealed class CancelMyOrderCommandhandler : IRequestHandler<CancelMyOrderCommand>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IOrderWriteRepository _orderWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CancelMyOrderCommandhandler(
            ICurrentUserService currentUserService,
            IProductWriteRepository productWriteRepository,
            IOrderWriteRepository orderWriteRepository,
            IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _productWriteRepository = productWriteRepository;
            _orderWriteRepository = orderWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(CancelMyOrderCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;

            var order = await _orderWriteRepository.GetOrderByIdAsync(request.OrderId, cancellationToken) ??
                throw new DomainNotFoundException("Order not found");

            if (order.UserId != currentUserId)
                throw new DomainUnauthorizedException("You can only cancel your own order.");

            // Return false if already cancelled so the handler can stop and avoid restoring stock again.
            var wasCancelled = order.CancelOrder();
            if (!wasCancelled) return;

            foreach (var item in order.OrderItems)
            {
                var product = await _productWriteRepository.GetProductByIdAsync(item.ProductId, cancellationToken) ??
                    throw new DomainNotFoundException("Product not found.");

                product.IncreaseStock(item.Quantity);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
