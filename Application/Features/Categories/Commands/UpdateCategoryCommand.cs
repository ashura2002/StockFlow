
using MediatR;

namespace Application.Features.Categories.Commands
{
    public sealed record UpdateCategoryCommand(
        Guid CategoryId, 
        string CategoryName, 
        string? Description) : IRequest;
}
