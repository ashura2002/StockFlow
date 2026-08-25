using Application.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Orders.Commands
{
    public sealed class ConfirmOrderCommandHandler : IRequestHandler<ConfirmOrderCommand>
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

        public async Task Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderWriteRepository.GetOrderByIdAsync(request.OrderId, cancellationToken) ??
                throw new DomainNotFoundException("Order not found");

            order.ConfirmOrder();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
