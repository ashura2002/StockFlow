using Application.Dtos;
using Application.Interfaces;
using Domain.ValueObjects;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class UserReadRepository : IUserReadRepository
    {

        public InventoryDbContext _context;

        public UserReadRepository(InventoryDbContext inventoryDbContext)
        {
            _context = inventoryDbContext;
        }

        public Task<IReadOnlyCollection<UserDto>> GetAllUsersAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsEmailExistAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .AnyAsync(u => u.Email == EmailVo.Create(email), cancellationToken);
        }
    }
}
