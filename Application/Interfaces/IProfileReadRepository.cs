using Application.Dtos;

namespace Application.Interfaces
{
    public interface IProfileReadRepository
    {
        Task<UserWithProfileResponseDto?> GetProfileAsync(Guid userId, CancellationToken cancellationToken);
    }
}
