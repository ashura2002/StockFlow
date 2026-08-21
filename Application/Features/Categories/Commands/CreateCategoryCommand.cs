using MediatR;

namespace Application.Features.Categories.Commands
{
    public sealed record CreateCategoryCommand(
        string CategoryName, 
        string? Descriptions) : IRequest<Guid>;
}
