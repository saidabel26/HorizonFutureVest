using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence.Entities;

namespace Persistence.Repositories
{
    public interface ICountryIndicatorRepository
    {
        Task<IEnumerable<CountryIndicator>> GetAllAsync();
        Task<CountryIndicator?> GetByIdAsync(int id);
        Task AddAsync(CountryIndicator entity);
        Task UpdateAsync(CountryIndicator entity);
        Task DeleteAsync(int id);
    }
}
