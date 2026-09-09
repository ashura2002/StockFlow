using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserWriteRepository
    {
        void Add(User user);
        void Remove(User user);
        Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
        Task<User?> GetUserByIdWithProfileAsync(Guid userId, CancellationToken cancellationToken);
        Task<User?> GetDeletedUserByEmailAsync(string email, CancellationToken cancellationToken);
    }
}
