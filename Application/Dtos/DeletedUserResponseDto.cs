using Domain.Enums;

namespace Application.Dtos
{
    public sealed record DeletedUserResponseDto(
        Guid UserId,
        string Email,
        Role Role,
        DateTime CreatedAt,
        DateTime? DeletedAt);
}