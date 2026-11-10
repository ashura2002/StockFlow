using Application.Common.Errors;
using Application.Common.Results;
using Application.Interfaces;
using Domain.Enums;
using MediatR;

namespace Application.Features.Orders.Commands
{
    public sealed class CancelMyOrderCommandhandler : IRequestHandler<CancelMyOrderCommand, Result>
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

        public async Task<Result> Handle(CancelMyOrderCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;

            var order = await _orderWriteRepository.GetOrderByIdAsync(request.OrderId, cancellationToken);
            if (order is null)
                return Result.Failure(OrderErrors.OrderNotFound);

            if (order.UserId != currentUserId)
                return Result.Failure(OrderErrors.CancelOwnOrder);

            // Return false if already cancelled so the handler can stop and avoid restoring stock again.
            var wasCancelled = order.CancelOrder(OrderCancellationSource.Customer);
            if (!wasCancelled) return Result.Success();

            foreach (var item in order.OrderItems)
            {
                var product = await _productWriteRepository.GetProductByIdAsync(item.ProductId, cancellationToken);
                if (product is null)
                    return Result.Failure(ProductErrors.ProductNotFound);

                product.IncreaseStock(item.Quantity);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
