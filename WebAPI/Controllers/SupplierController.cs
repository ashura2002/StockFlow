using Application.Dtos;
using Application.Features.Suppliers.Commands;
using Application.Features.Suppliers.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Constants;
using WebAPI.RequestDtos;

namespace WebAPI.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    [Authorize(Roles = RolesConstant.Admin)]
    public class SupplierController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SupplierController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        public async Task<ActionResult<Guid>> CreateSupplier([FromBody] CreateSupplierRequest request,
            CancellationToken ct)
        {
            var command = new CreateSupplierCommand(
                request.SupplierName,
                request.Email,
                request.PhoneNumber,
                request.Address);

            var result = await _mediator.Send(command, ct);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<SupplierResponseDto>>> GetAllSuppliers(
            [FromQuery] PaginatedRequest request, CancellationToken cancellationToken)
        {
            var query = new GetAllSuppliersQuery(request.Page, request.PageSize);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPatch("{supplierId:guid}")]
        public async Task<ActionResult> UpdateSupplier(
            Guid supplierId,
            [FromBody] UpdateSupplierRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateSupplierCommand(
                supplierId,
                request.SupplierName,
                request.Email,
                request.PhoneNumber,
                request.Address);

            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{supplierId:guid}")]
        public async Task<ActionResult> DeleteSupplier(Guid supplierId, CancellationToken cancellationToken)
        {
            var command = new DeleteSupplierCommand(supplierId);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}

// final endpoint get supplier by id included with list of products then refactor the post method