using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICountryIndicatorService
{
    Task<IEnumerable<CountryIndicatorDto>> GetAllAsync();
    Task<CountryIndicatorDto?> GetByIdAsync(int id);
    Task AddAsync(CountryIndicatorDto dto);
    Task UpdateAsync(CountryIndicatorDto dto);
    Task DeleteAsync(int id);
}
