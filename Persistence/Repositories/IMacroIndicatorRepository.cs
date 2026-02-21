using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence.Entities;

namespace Persistence.Repositories
{
    public interface IMacroIndicatorRepository
    {
        Task<IEnumerable<MacroIndicator>> GetAllAsync();
        Task<MacroIndicator?> GetByIdAsync(int id);
        Task AddAsync(MacroIndicator entity);
        Task UpdateAsync(MacroIndicator entity);
        Task DeleteAsync(int id);
    }
}
