using Application.Common.Errors;
using Application.Common.Results;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Orders.Commands
{
    public sealed class ConfirmOrderCommandHandler : IRequestHandler<ConfirmOrderCommand, Result>
    {
        private readonly IOrderWriteRepository _orderWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmOrderCommandHandler(
            IOrderWriteRepository orderWriteRepository,
            IUnitOfWork unitOfWork)
        {
            _orderWriteRepository = orderWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderWriteRepository.GetOrderByIdAsync(request.OrderId, cancellationToken);
            if (order is null)
                return Result.Failure(OrderErrors.OrderNotFound);

            order.ConfirmOrder();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
