using System.Collections.Generic;
using System.Threading.Tasks;
using Persistence.Entities;

namespace Persistence.Repositories
{
    public interface ICountryRepository
    {
        Task<IEnumerable<Country>> GetAllAsync();
        Task<Country?> GetByIdAsync(int id);
        Task AddAsync(Country entity);
        Task UpdateAsync(Country entity);
        Task DeleteAsync(int id);
    }
}
