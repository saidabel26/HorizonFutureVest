using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using Persistence.Entities;

namespace Persistence.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly AppDbContext _context;

        public CountryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Country>> GetAllAsync()
        {
            return await _context.Countries.ToListAsync();
        }

        public async Task<Country?> GetByIdAsync(int id)
        {
            return await _context.Countries.FindAsync(id);
        }

        public async Task AddAsync(Country entity)
        {
            _context.Countries.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Country entity)
        {
            var existing = await _context.Countries.FindAsync(entity.Id);
            if (existing != null)
            {
                existing.Name = entity.Name;
                existing.IsoCode = entity.IsoCode;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Countries.FindAsync(id);
            if (entity != null)
            {
                _context.Countries.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
