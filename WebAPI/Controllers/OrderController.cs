using Application.Dtos;
using Application.Features.Orders.Commands;
using Application.Features.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using WebAPI.Constants;
using WebAPI.RequestDtos;

namespace WebAPI.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = RolesConstant.Customer)]
        public async Task<ActionResult<Guid>> CreateOrder(
            [FromBody] CreateOrderRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateOrderCommand(request.OrderItems);
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{orderId:guid}")]
        [Authorize(Roles = RolesConstant.Admin)]
        [EnableRateLimiting("GetResourcesPolicy")]
        public async Task<ActionResult<AdminOrderResponseDto>> GetOrderByIdByAdmin(
            Guid orderId,
            CancellationToken cancellationToken)
        {
            var query = new GetOrderByIdByAdminQuery(orderId);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("pending-orders")]
        [Authorize(Roles = RolesConstant.Admin)]
        [EnableRateLimiting("GetResourcesPolicy")]
        public async Task<ActionResult<IReadOnlyCollection<AdminOrderResponseDto>>> GetAllPendingOrders(
            [FromQuery] PaginatedRequest request,
            CancellationToken cancellationToken)
        {
            var query = new GetAllPendingOrdersQuery(request.Page, request.PageSize);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("confirmed-orders")]
        [Authorize(Roles = RolesConstant.Admin)]
        [EnableRateLimiting("GetResourcesPolicy")]
        public async Task<ActionResult<IReadOnlyCollection<AdminOrderResponseDto>>> GetAllConfirmedOrders(
            [FromQuery] PaginatedRequest request,
            CancellationToken cancellationToken)
        {
            var query = new GetAllConfirmOrdersQuery(request.Page, request.PageSize);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("cancelled-orders")]
        [Authorize(Roles = RolesConstant.Admin)]
        [EnableRateLimiting("GetResourcesPolicy")]
        public async Task<ActionResult<IReadOnlyCollection<AdminOrderResponseDto>>> GetAllCancelledOrders(
            [FromQuery] PaginatedRequest request,
            CancellationToken cancellationToken)
        {
            var query = new GetAllCancelledOrdersQuery(request.Page, request.PageSize);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("completed-orders")]
        [Authorize(Roles = RolesConstant.Admin)]
        [EnableRateLimiting("GetResourcesPolicy")]
        public async Task<ActionResult<IReadOnlyCollection<AdminOrderResponseDto>>> GetAllCompletedOrders(
          [FromQuery] PaginatedRequest request,
          CancellationToken cancellationToken)
        {
            var query = new GetAllCompletedOrdersQuery(request.Page, request.PageSize);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }


        [HttpPatch("{orderId:guid}/confirm")]
        [Authorize(Roles = RolesConstant.Admin)]
        public async Task<ActionResult> ConfirmOrder(Guid orderId, CancellationToken cancellationToken)
        {
            var command = new ConfirmOrderCommand(orderId);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpPatch("my-orders/{orderId:guid}/cancel")]
        [Authorize(Roles = RolesConstant.Customer)]
        public async Task<ActionResult> CancelMyOrder(Guid orderId, CancellationToken cancellationToken)
        {
            var command = new CancelMyOrderCommand(orderId);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpPatch("{orderId:guid}/cancel")]
        [Authorize(Roles = RolesConstant.Admin)]
        public async Task<ActionResult> CancelOrderByAdmin(Guid orderId, CancellationToken cancellationToken)
        {
            var command = new CancelOrderByAdminCommand(orderId);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpGet("my-orders")]
        [Authorize(Roles = RolesConstant.Customer)]
        [EnableRateLimiting("GetResourcesPolicy")]
        public async Task<ActionResult<IReadOnlyCollection<CustomerOrderResponseDto>>> GetAllMyOrders(
            [FromQuery] PaginatedRequest request,
            CancellationToken cancellationToken)
        {
            var query = new GetAllMyOrderQuery(request.Page, request.PageSize);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }


        [HttpGet("my-orders{orderId:guid}/details")]
        [Authorize(Roles = RolesConstant.Customer)]
        [EnableRateLimiting("GetResourcesPolicy")]
        public async Task<ActionResult<CustomerOrderResponseDto>> GetMyOrderById(Guid orderId, CancellationToken cancellationToken)
        {
            var query = new GetMyOrderByIdQuery(orderId);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }


        [HttpPatch("my-orders/{orderId:guid}")]
        [Authorize(Roles = RolesConstant.Customer)]
        public async Task<ActionResult> UpdateOrder(
            Guid orderId,
            [FromBody] UpdateOrderItemRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateMyOrderCommand(orderId, request.OrderItems);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpPatch("{orderId:guid}/complete")]
        [Authorize(Roles = RolesConstant.Admin)]
        public async Task<ActionResult> CompleteOrder(Guid orderId, CancellationToken cancellationToken)
        {
            var command = new CompleteOrderCommand(orderId);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}

