using Domain.Enums;

namespace Application.Interfaces
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }
        Role Role { get; }
    }
}
