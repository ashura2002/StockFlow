using Domain.Enums;namespace Application.Dtos
{
    public sealed record UserResponseDto(
        Guid UserId,
        string Email,
        Role Role,
        DateTime CreatedAt);
}
