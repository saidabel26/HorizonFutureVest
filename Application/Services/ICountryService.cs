using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICountryService
{
    Task<IEnumerable<CountryDto>> GetAllAsync();
    Task<CountryDto?> GetByIdAsync(int id);
    Task AddAsync(CountryDto dto);
    Task UpdateAsync(CountryDto dto);
    Task DeleteAsync(int id);
}
