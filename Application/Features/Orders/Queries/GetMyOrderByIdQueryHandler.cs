using Application.Common.Errors;
using Application.Common.Results;
using Application.Dtos;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Orders.Queries
{
    public sealed class GetMyOrderByIdQueryHandler : IRequestHandler<GetMyOrderByIdQuery, ResultWithValue<CustomerOrderResponseDto>>
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

        public async Task<ResultWithValue<CustomerOrderResponseDto>> Handle(GetMyOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;
            var order = await _orderReadRepository.GetMyOrderByIdAsync(request.OrderId, currentUserId, cancellationToken);
            if (order is null)
                return ResultWithValue<CustomerOrderResponseDto>.Failure(OrderErrors.OrderNotFound);

            return ResultWithValue<CustomerOrderResponseDto>.Success(order);
        }
    }
}
