using Application.Dtos;
using Application.Features.Products.Commands;
using Application.Features.Products.Queries;
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
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = RolesConstant.Admin)]
        public async Task<ActionResult<Guid>> CreateProduct(
            [FromBody] CreateProductRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateProductCommand(
                request.ProductName,
                request.Price,
                request.Stock,
                request.CategoryId,
                request.SupplierId,
                request.ProductDescriptions);

            var result = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(
                nameof(GetProductById),
                new { productId = result },
                result);
        }

        [HttpGet]
        [EnableRateLimiting("GetResourcesPolicy")]
        public async Task<ActionResult<IReadOnlyCollection<ProductResponseDto>>> GetAllProducts(
            [FromQuery] PaginatedRequest request,
            CancellationToken cancellationToken)
        {
            var query = new GetAllProductsQuery(request.Page, request.PageSize);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("details/{productId:guid}")]
        [EnableRateLimiting("GetResourcesPolicy")]
        public async Task<ActionResult<ProductResponseDto>> GetProductById(
            Guid productId,
            CancellationToken cancellationToken)
        {
            var query = new GetProductByIdQuery(productId);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [Authorize(Roles = RolesConstant.Admin)]
        [HttpPatch("{productId:guid}")]
        public async Task<ActionResult> UpdateProduct(
            Guid productId,
            [FromBody] UpdateProductRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateProductCommand(
                productId,
                request.ProductName,
                request.Price,
                request.Stock,
                request.Descriptions);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{productId:guid}")]
        [Authorize(Roles = RolesConstant.Admin)]
        public async Task<ActionResult> DeleteProduct(
            Guid productId,
            CancellationToken cancellationToken)
        {
            var command = new DeleteProductCommand(productId);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpGet("search")]

        [EnableRateLimiting("GetResourcesPolicy")]
        public async Task<ActionResult<ProductResponseDto>> SearchProductByName(
            [FromQuery] SearchProductByNameRequest request,
            CancellationToken cancellationToken)
        {
            var query = new SearchProductByNameQuery(
                request.ProductName,
                request.Page,
                request.PageSize);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("deleted")]
        [Authorize(Roles = RolesConstant.Admin)]
        public async Task<ActionResult<IReadOnlyCollection<DeletedProductResponseDto>>> GetAllDeletedProducts(
            CancellationToken cancellationToken)
        {
            var query = new GetAllDeletedProductQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPatch("{productId:guid}/product-image")]
        [Authorize(Roles = RolesConstant.Admin)]
        public async Task<ActionResult<UploadedImage>> UpdateProductImage(
            Guid productId, 
            IFormFile file, 
            CancellationToken cancellationToken)
        {
            await using var stream = file.OpenReadStream();

            var result = await _mediator.Send(
                new UpdateProductImageCommand(
                    productId,
                    stream,
                    file.FileName,
                    file.ContentType,
                    file.Length),
                cancellationToken);

            return Ok(result);
        }
    }
}
