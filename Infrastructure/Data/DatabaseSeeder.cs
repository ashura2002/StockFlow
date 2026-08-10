using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Data
{
    public class DatabaseSeeder
    {
        private readonly InventoryDbContext _context;
        private readonly IPasswordService _passwordhasher;
        private readonly SeededUserSettings _seededUserSetting;
        private readonly ILogger<DatabaseSeeder> _logger;


        public DatabaseSeeder(
            InventoryDbContext context,
            IPasswordService passwordhasher,
            IOptions<SeededUserSettings> options,
            ILogger<DatabaseSeeder> logger)
        {
            _context = context;
            _passwordhasher = passwordhasher;
            _seededUserSetting = options.Value;
            _logger = logger;
        }


        public async Task SeedAdminUser(CancellationToken cancellationToken = default)
        {
            bool existingUser = await _context.Users.AnyAsync(cancellationToken);
            if (existingUser) return;

            EmailVo emailVo = EmailVo.Create(_seededUserSetting.Email);
            string password = _passwordhasher.HashPassword(_seededUserSetting.Password);
            PasswordVo passwordVo = PasswordVo.Create(password);

            User admin = User.Create(emailVo, Role.Admin, passwordVo);

            _context.Users.Add(admin);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Seeding admin... {admin}", admin.Id);
        }
    }
}
