using Application.Dtos;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Orders.Queries
{
    public sealed class GetAllCompletedOrdersQueryHandler : IRequestHandler<GetAllCompletedOrdersQuery, IReadOnlyCollection<AdminOrderResponseDto>>
    {
        private readonly IOrderReadRepository _orderReadRepository;

        public GetAllCompletedOrdersQueryHandler(IOrderReadRepository orderReadRepository)
        {
            _orderReadRepository = orderReadRepository;
        }

        public async Task<IReadOnlyCollection<AdminOrderResponseDto>> Handle(GetAllCompletedOrdersQuery request, CancellationToken cancellationToken)
        {
            return await _orderReadRepository.GetAllCompletedOrdersAsync(
                request.Page, 
                request.PageSize, 
                cancellationToken);
        }
    }
}
