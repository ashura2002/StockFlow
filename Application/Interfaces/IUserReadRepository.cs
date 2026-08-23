using Application.Dtos;


namespace Application.Interfaces
{
    public interface IUserReadRepository
    {
        Task<IReadOnlyCollection<UserResponseDto>> GetAllActiveUsersAsync(int page, int pageSize, CancellationToken cancellationToken);
        Task<IReadOnlyCollection<DeletedUserResponseDto>> GetAllDeletedUsersAsync(CancellationToken cancellationToken);
        Task<bool> IsEmailExistAsync(string email, CancellationToken cancellationToken);
        Task<UserResponseDto?> GetAdminAsync(CancellationToken cancellationToken);
    }
}
