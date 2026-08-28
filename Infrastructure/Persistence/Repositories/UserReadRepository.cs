using Application.Dtos;
using Application.Interfaces;
using Domain.Enums;
using Domain.ValueObjects;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistence.Repositories
{
    public sealed class UserReadRepository : IUserReadRepository
    {

        private readonly InventoryDbContext _context;

        public UserReadRepository(InventoryDbContext inventoryDbContext)
        {
            _context = inventoryDbContext;
        }

        public async Task<UserResponseDto?> GetAdminAsync(CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Role == Role.Admin)
                .Select(u => 
                    new UserResponseDto(
                    u.Id,
                    u.Email.Value, 
                    u.Role, 
                    u.CreatedAt))
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<UserResponseDto>> GetAllActiveUsersAsync(int page, int pageSize, CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .OrderByDescending(u => u.CreatedAt)
                .Select(u => 
                new UserResponseDto(
                    u.Id, 
                    u.Email.Value, 
                    u.Role, 
                    u.CreatedAt))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<DeletedUserResponseDto>> GetAllDeletedUsersAsync(CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .IgnoreQueryFilters()
                .Where(u => u.DeletedAt.HasValue)
                .OrderByDescending(u => u.CreatedAt)
                .Select(u => new DeletedUserResponseDto(
                    u.Id, 
                    u.Email.Value, 
                    u.Role, 
                    u.CreatedAt,
                    u.DeletedAt))
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<UserResponseDto>> GetUserByEmailAsync(
            string email,
            int page, 
            int pageSize, 
            CancellationToken cancellationToken)
        {
            return await _context.Database.SqlQuery<UserResponseDto>($"""
                    SELECT 
                        u."Id" AS "UserId",
                        u."Email" AS "Email",
                        u."Role" AS "Role",
                        u."CreatedAt" AS "CreatedAt"
                    FROM "Users" AS u
                        WHERE u."DeletedAt" IS NULL
                        AND u."Email" ILIKE '%' || {email} || '%'
                        ORDER BY u."CreatedAt" DESC
                        LIMIT {pageSize}
                        OFFSET {(page - 1 ) * pageSize}
                """).ToListAsync(cancellationToken);
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .Select(u => 
                new UserResponseDto(
                    u.Id, 
                    u.Email.Value, 
                    u.Role, 
                    u.CreatedAt))
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> IsEmailExistAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .AnyAsync(u => u.Email == EmailVo.Create(email), cancellationToken);
        }
    }
}
