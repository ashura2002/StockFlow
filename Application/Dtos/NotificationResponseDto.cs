
namespace Application.Dtos
{
    public record NotificationResponseDto(Guid NotificationId, Guid UserId, string Content, bool IsRead);
}
