
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Orders.Commands
{
    public sealed class CompleteOrderCommandHandler : IRequestHandler<CompleteOrderCommand>
    {
        private readonly IOrderWriteRepository _orderWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CompleteOrderCommandHandler(
            IOrderWriteRepository orderWriteRepository, 
            IUnitOfWork unitOfWork)
        {
            _orderWriteRepository = orderWriteRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(CompleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderWriteRepository.GetOrderByIdAsync(request.OrderId, cancellationToken) ??
                throw new DomainNotFoundException("Order not found.");

            order.CompleteOrder();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
