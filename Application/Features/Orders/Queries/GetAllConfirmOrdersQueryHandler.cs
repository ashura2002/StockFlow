using Application.Dtos;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Orders.Queries
{
    public sealed class GetAllConfirmOrdersQueryHandler : IRequestHandler<GetAllConfirmOrdersQuery, IReadOnlyCollection<AdminOrderResponseDto>>
    {
        private readonly IOrderReadRepository _orderReadRepository;
        public GetAllConfirmOrdersQueryHandler(IOrderReadRepository orderReadRepository)
        {
            _orderReadRepository = orderReadRepository;
        }

        public async Task<IReadOnlyCollection<AdminOrderResponseDto>> Handle(GetAllConfirmOrdersQuery request, CancellationToken cancellationToken)
        {
            return await _orderReadRepository.GetAllConfirmOrdersAsync(
                request.Page, 
                request.PageSize, 
                cancellationToken);
        }
    }
}
