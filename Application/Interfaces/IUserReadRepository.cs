using Application.Dtos;


namespace Application.Interfaces
{
    public interface IUserReadRepository
    {
        Task<IReadOnlyCollection<UserDto>> GetAllUsersAsync(int page, int pageSize, CancellationToken cancellationToken);
        Task<bool> IsEmailExistAsync(string email, CancellationToken cancellationToken);
    }
}
