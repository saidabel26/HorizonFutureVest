using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using Persistence.Entities;

namespace Persistence.Repositories
{
    public class ReturnRateConfigRepository : IReturnRateConfigRepository
    {
        private readonly AppDbContext _context;

        public ReturnRateConfigRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ReturnRateConfig?> GetConfigAsync()
        {
            return await _context.ReturnRateConfigs.FirstOrDefaultAsync();
        }

        public async Task UpdateConfigAsync(ReturnRateConfig entity)
        {
            var existing = await _context.ReturnRateConfigs.FirstOrDefaultAsync();
            if (existing != null)
            {
                existing.MinRate = entity.MinRate;
                existing.MaxRate = entity.MaxRate;
                _context.ReturnRateConfigs.Update(existing);
            }
            else
            {
                // Solo si no existe ninguna configuración, crea una nueva
                _context.ReturnRateConfigs.Add(entity);
            }
            await _context.SaveChangesAsync();
        }
    }
}
