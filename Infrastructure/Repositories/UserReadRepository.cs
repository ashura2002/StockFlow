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

        public async Task<IReadOnlyCollection<UserDto>> GetAllUsersAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .OrderByDescending(u => u.CreatedAt)
                .Select(u => 
                new UserDto(u.Id, 
                u.Email.Value, 
                u.Role, 
                u.CreatedAt))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> IsEmailExistAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .AnyAsync(u => u.Email == EmailVo.Create(email), cancellationToken);
        }
    }
}
