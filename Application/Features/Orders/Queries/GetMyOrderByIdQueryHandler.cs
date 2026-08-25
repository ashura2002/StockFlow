using Application.Dtos;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Orders.Queries
{
    public sealed class GetMyOrderByIdQueryHandler : IRequestHandler<GetMyOrderByIdQuery, CustomerOrderResponseDto>
    {
        private readonly IOrderReadRepository _orderReadRepository;
        private readonly ICurrentUserService _currentUserService;
        public GetMyOrderByIdQueryHandler(
            IOrderReadRepository orderReadRepository,
            ICurrentUserService currentUserService)
        {
            _orderReadRepository = orderReadRepository;
            _currentUserService = currentUserService;
        }

        public async Task<CustomerOrderResponseDto> Handle(GetMyOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;
            return await _orderReadRepository.GetMyOrderByIdAsync(request.OrderId, currentUserId, cancellationToken) ??
                 throw new DomainNotFoundException("Order not found.");
        }
    }
}
