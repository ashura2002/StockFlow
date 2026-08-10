using Domain.Enums;

namespace Application.Dtos
{
    public record UserDto(
        Guid UserId,
        string Email,
        Role Role,
        DateTime CreatedAt);
}
