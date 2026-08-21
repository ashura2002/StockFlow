using Application.Features.Products.Commands;
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
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreaeProduct([FromBody] CreateProductRequest request, CancellationToken ct)
        {
            var command = new CreateProductCommand(
                request.ProductName, 
                request.Price, 
                request.Stock, 
                request.CategoryId, 
                request.SupplierId, 
                request.ProductDescriptions);

            var result = await _mediator.Send(command, ct);
            return Ok(result);
        }

    }
}
