using Application.Dtos;


namespace Application.Interfaces
{
    public interface IUserReadRepository
    {
        Task<IReadOnlyCollection<UserDto>> GetAllActiveUsersAsync(int page, int pageSize, CancellationToken cancellationToken);
        Task<IReadOnlyCollection<UserDto>> GetAllInActiveUsersAsync(CancellationToken cancellationToken);
        Task<bool> IsEmailExistAsync(string email, CancellationToken cancellationToken);
    }
}
