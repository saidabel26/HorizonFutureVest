using System.Collections.Generic;
using System.Threading.Tasks;

public interface IMacroIndicatorService
{
    Task<IEnumerable<MacroIndicatorDto>> GetAllAsync();
    Task<MacroIndicatorDto?> GetByIdAsync(int id);
    Task AddAsync(MacroIndicatorDto dto);
    Task UpdateAsync(MacroIndicatorDto dto);
    Task DeleteAsync(int id);
}
