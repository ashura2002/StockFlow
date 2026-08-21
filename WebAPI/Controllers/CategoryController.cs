using Application.Dtos;
using Application.Features.Categories.Commands;
using Application.Features.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Constants;
using WebAPI.RequestDtos;

namespace WebAPI.Controllers
{
    [Route("api/Categories")]
    [ApiController]
    [Authorize(Roles = RolesConstant.Admin)]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateCategory(
            [FromBody] CreateCategoryRequest request, 
            CancellationToken cancellationToken)
        {
            var command = new CreateCategoryCommand(request.CategoryName, request.Description);
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<CategoryResponseDto>>> GetAllCategories(
            CancellationToken cancellationToken)
        {
            var query = new GetAllCategoriesQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPatch("{categoryId:guid}")]
        public async Task<ActionResult> UpdateCategory(
            Guid categoryId, [
            FromBody] UpdateCategoryRequest request, 
            CancellationToken cancellationToken)
        {
            var command = new UpdateCategoryCommand(categoryId, request.CategoryName, request.Description);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{categoryId:guid}")]
        public async Task<ActionResult> DeleteCategory(Guid categoryId, CancellationToken cancellationToken)
        {
            var command = new DeleteCategoryCommand(categoryId);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
// final endpoint get by id included with list of products then refactor the post method