namespace Application.Dtos
{
    public sealed record UserWithProfileResponseDto(
        Guid UserId,
        string Email,
        string FistName,
        string LastName,
        DateOnly DateOfBirth,
        string Address,
        string? ProfilePictureUrl,
        string? ProfilePicturePublicId);
}
