using Domain.Enums;namespace Application.Dtos
{
    public record UserResponseDto(
        Guid UserId,
        string Email,
        Role Role,
        DateTime CreatedAt);
}
