using Application.Dtos;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Orders.Queries
{
    public sealed class GetAllMyOrderQueryHandler : IRequestHandler<GetAllMyOrderQuery, IReadOnlyCollection<CustomerOrderResponseDto>>
    {
        private readonly IOrderReadRepository _orderReadRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetAllMyOrderQueryHandler(
            IOrderReadRepository orderReadRepository,
            ICurrentUserService currentUserService)
        {
            _orderReadRepository = orderReadRepository;
            _currentUserService = currentUserService;
        }

        public async Task<IReadOnlyCollection<CustomerOrderResponseDto>> Handle(GetAllMyOrderQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;

            return await _orderReadRepository.GetAllMyOrdersAsync(
                request.Page, 
                request.PageSize, 
                currentUserId, 
                cancellationToken);
        }
    }
}
