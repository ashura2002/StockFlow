using Application.Dtos;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Orders.Queries
{
    public sealed class GetAllCancelledOrdersQueryHandler : IRequestHandler<GetAllCancelledOrdersQuery, IReadOnlyCollection<AdminOrderResponseDto>>
    {
        private readonly IOrderReadRepository _orderReadRepository;
        public GetAllCancelledOrdersQueryHandler(IOrderReadRepository orderReadRepository)
        {
            _orderReadRepository = orderReadRepository;
        }


        public async Task<IReadOnlyCollection<AdminOrderResponseDto>> Handle(GetAllCancelledOrdersQuery request, CancellationToken cancellationToken)
        {
            return await _orderReadRepository.GetAllCancelledOrdersAsync(
                request.Page, 
                request.PageSize, 
                cancellationToken);
        }
    }
}
