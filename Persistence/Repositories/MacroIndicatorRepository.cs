using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using Persistence.Entities;

namespace Persistence.Repositories
{
    public class MacroIndicatorRepository : IMacroIndicatorRepository
    {
        private readonly AppDbContext _context;

        public MacroIndicatorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MacroIndicator>> GetAllAsync()
        {
            return await _context.MacroIndicators.ToListAsync();
        }

        public async Task<MacroIndicator?> GetByIdAsync(int id)
        {
            return await _context.MacroIndicators.FindAsync(id);
        }

        public async Task AddAsync(MacroIndicator entity)
        {
            _context.MacroIndicators.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MacroIndicator entity)
        {
            var existing = await _context.MacroIndicators.FindAsync(entity.Id);
            if (existing != null)
            {
                existing.Name = entity.Name;
                existing.Weight = entity.Weight;
                existing.IsHigherBetter = entity.IsHigherBetter;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.MacroIndicators
                .Include(m => m.CountryIndicators)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (entity == null)
                return;

            if (entity.CountryIndicators != null && entity.CountryIndicators.Any())
                throw new InvalidOperationException("No se puede eliminar el macroindicador porque está siendo utilizado por uno o más indicadores de país.");

            _context.MacroIndicators.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
