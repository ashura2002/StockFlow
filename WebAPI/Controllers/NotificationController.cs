using Application.Dtos;
using Application.Features.Notifications.Commands;
using Application.Features.Notifications.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Extensions;

namespace WebAPI.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPatch("{notificationId:guid}/read")]
        public async Task<ActionResult> MarkAsRead(Guid notificationId, CancellationToken cancellationToken)
        {
            var command = new MarkAsReadCommand(notificationId);
            var result = await _mediator.Send(command, cancellationToken);
            return this.ToActionResult(result);
        }

        [HttpDelete("{notificationId:guid}")]
        public async Task<ActionResult> DeleteNotificationById(Guid notificationId, CancellationToken cancellationToken)
        {
            var command = new DeleteNotificationByIdCommand(notificationId);
            var result = await _mediator.Send(command, cancellationToken);
            return this.ToActionResult(result);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<NotificationResponseDto>>> GetAllNotifications(CancellationToken cancellationToken)
        {
            var query = new GetAllNotificationQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return this.ToActionResult(result);
        }

    }
}
