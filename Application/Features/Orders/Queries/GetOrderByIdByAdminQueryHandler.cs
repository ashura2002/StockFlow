using Application.Dtos;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Orders.Queries
{
    public sealed class GetOrderByIdByAdminQueryHandler : IRequestHandler<GetOrderByIdByAdminQuery, AdminOrderResponseDto>
    {
        private readonly IOrderReadRepository _orderReadRepository;
            
        public GetOrderByIdByAdminQueryHandler(IOrderReadRepository orderReadRepository)
        {
            _orderReadRepository = orderReadRepository;
        }

        public async Task<AdminOrderResponseDto> Handle(GetOrderByIdByAdminQuery request, CancellationToken cancellationToken)
        {
           return await _orderReadRepository.GetOrderByIdAsync(request.OrderId, cancellationToken)??
                throw new DomainNotFoundException("Order not found");
        }
    }
}
