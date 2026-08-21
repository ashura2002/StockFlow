
namespace Application.Dtos
{
    public sealed record CategoryResponseDto(
        Guid CategoryId,
        string CategoryName,
        string? Description);
}
