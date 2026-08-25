using Application.Dtos;
using Application.Features.Orders.Commands;
using Application.Features.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<IReadOnlyCollection<AdminOrderResponseDto>>> GetAllPendingOrders(
            [FromQuery] PaginatedRequest request,
            CancellationToken cancellationToken)
        {
            var query = new GetAllPendingOrdersQuery(request.Page, request.PageSize);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPatch("confirm/{orderId:guid}")]
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
        public async Task<ActionResult<CustomerOrderResponseDto>> GetMyOrderById(Guid orderId, CancellationToken cancellationToken)
        {
            var query = new GetMyOrderByIdQuery(orderId);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
         // update order
         // delete order
        // search user by email to practice raw sql
    }
}
