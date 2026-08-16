using Application.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistence.Repositories
{
    public sealed class UserWriteRepository : IUserWriteRepository
    {
        private readonly InventoryDbContext _context;

        public UserWriteRepository(InventoryDbContext inventoryDbContext)
        {
            _context = inventoryDbContext;
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
        }

        public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Users
                 .FirstOrDefaultAsync(u => u.Email== EmailVo.Create(email),
                 cancellationToken);
        }

        public async Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId,
                cancellationToken);
        }

        public async Task<User?> GetUserByIdWithProfileAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Users
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Id == userId,
                cancellationToken);
        }

        public void Remove(User user)
        {
            _context.Users.Remove(user);
        }
    }
}
