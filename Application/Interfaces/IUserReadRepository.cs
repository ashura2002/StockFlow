using Application.Dtos;
using Domain.ValueObjects;


namespace Application.Interfaces
{
    public interface IUserReadRepository
    {
        Task<IReadOnlyCollection<UserResponseDto>> GetAllActiveUsersAsync(int page, int pageSize, CancellationToken cancellationToken);
        Task<IReadOnlyCollection<DeletedUserResponseDto>> GetAllDeletedUsersAsync(CancellationToken cancellationToken);
        Task<bool> IsEmailExistAsync(string email, CancellationToken cancellationToken);
        Task<UserResponseDto?> GetAdminAsync(CancellationToken cancellationToken);
        Task<IReadOnlyCollection<UserResponseDto>> GetUserByEmailAsync(string email, int page, int pageSize, CancellationToken cancellationToken);
        Task<UserResponseDto?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);
    }
}
