using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Persistence.Repositories
{
    public sealed class ProfileWriteRepository : IProfileWriteRepository
    {
        private readonly InventoryDbContext _context;
        public ProfileWriteRepository(InventoryDbContext inventoryDbContext)
        {
            _context = inventoryDbContext;
        }

        public void Add(Profile profile)
        {
            _context.Profiles.Add(profile);
        }

        public void Remove(Profile profile)
        {
            _context.Profiles.Remove(profile);
        }
    }
}
