using Application.Common.Errors;
using Application.Common.Results;
using Application.Dtos;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Orders.Queries
{
    public sealed class GetOrderByIdByAdminQueryHandler : IRequestHandler<GetOrderByIdByAdminQuery, ResultWithValue<AdminOrderResponseDto>>
    {
        private readonly IOrderReadRepository _orderReadRepository;

        public GetOrderByIdByAdminQueryHandler(IOrderReadRepository orderReadRepository)
        {
            _orderReadRepository = orderReadRepository;
        }

        public async Task<ResultWithValue<AdminOrderResponseDto>> Handle(GetOrderByIdByAdminQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderReadRepository.GetOrderByIdAsync(request.OrderId, cancellationToken);

            if (order is null)
                return ResultWithValue<AdminOrderResponseDto>.Failure(OrderErrors.OrderNotFound);

            return ResultWithValue<AdminOrderResponseDto>.Success(order);
        }
    }
}
