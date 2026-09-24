using Domain.Entities;

namespace Application.Interfaces
{
    public interface IRefreshTokenWriteRepository
    {
        void Add(RefreshToken refreshToken);
    }
}
