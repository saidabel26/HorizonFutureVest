using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using Persistence.Entities;

namespace Persistence.Repositories
{
    public class CountryIndicatorRepository : ICountryIndicatorRepository
    {
        private readonly AppDbContext _context;

        public CountryIndicatorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CountryIndicator>> GetAllAsync()
        {
            return await _context.CountryIndicators
                .Include(ci => ci.Country)
                .Include(ci => ci.MacroIndicator)
                .ToListAsync();
        }

        public async Task<CountryIndicator?> GetByIdAsync(int id)
        {
            return await _context.CountryIndicators
                .Include(ci => ci.Country)
                .Include(ci => ci.MacroIndicator)
                .FirstOrDefaultAsync(ci => ci.Id == id);
        }

        public async Task AddAsync(CountryIndicator entity)
        {
            _context.CountryIndicators.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CountryIndicator entity)
        {
            _context.CountryIndicators.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.CountryIndicators.FindAsync(id);
            if (entity != null)
            {
                _context.CountryIndicators.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
