using Application.Dtos;
using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public sealed class ProfileReadRepository : IProfileReadRepository
    {
        private readonly InventoryDbContext _context;

        public ProfileReadRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<UserWithProfileResponseDto?> GetProfileAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Profiles
                .AsNoTracking()
                .Where(p => p.UserId == userId)
                .Select(p => new UserWithProfileResponseDto(
                    p.UserId,
                    p.User.Email.Value,
                    p.FirstName.Value,
                    p.LastName.Value,
                    p.DateOfBirth,
                    p.Address.Value,
                    p.ProfilePictureUrl,
                    p.ProfilePicturePublicId))
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
