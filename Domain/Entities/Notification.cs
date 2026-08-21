
namespace Domain.Entities
{
    public class Notification : BaseEntity
    {
        public Guid UserId { get; private set; }
        public User? User { get; private set; } = null!;
        public string Content { get; private set; }
        public bool IsRead { get; private set; } = false;

        private Notification(Guid userId, string content, bool isRead = false)
        {
            UserId = userId;
            Content = content;
            IsRead = isRead;
        }


        public static Notification Create(Guid userId, string content, bool isRead = false)
        {
            return new Notification(userId, content, isRead);
        }

        public void MarkAsRead()
        {
            if (IsRead) return;

            IsRead = true;
            Touch();
        }
    }
}