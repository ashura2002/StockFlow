using Application.Common.Errors;
using Application.Common.Results;
using Application.Interfaces;
using Domain.Enums;
using MediatR;
namespace Application.Features.Orders.Commands
{
    public sealed class CancelOrderByAdminCommandHandler : IRequestHandler<CancelOrderByAdminCommand, Result>
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

        public async Task<Result> Handle(CancelOrderByAdminCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderWriteRepository.GetOrderByIdAsync(request.OrderId, cancellationToken);
            if (order is null)
                return Result.Failure(OrderErrors.OrderNotFound);

            var isOrderCancel = order.CancelOrder(OrderCancellationSource.Admin);
            if (!isOrderCancel) return Result.Success();

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
