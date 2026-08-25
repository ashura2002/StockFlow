using Application.Dtos;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Orders.Queries
{
    public sealed class GetAllPendingOrdersQueryHandler : IRequestHandler<GetAllPendingOrdersQuery, IReadOnlyCollection<AdminOrderResponseDto>>
    {
        private readonly IOrderReadRepository _orderReadRepository;

        public GetAllPendingOrdersQueryHandler(IOrderReadRepository orderReadRepository)
        {
            _orderReadRepository = orderReadRepository;
        }

        public async Task<IReadOnlyCollection<AdminOrderResponseDto>> Handle(GetAllPendingOrdersQuery request, CancellationToken cancellationToken)
        {
            return await _orderReadRepository.GetAllPendingOrdersAsync(
                request.Page, 
                request.PageSize, 
                cancellationToken);
        }
    }
}
