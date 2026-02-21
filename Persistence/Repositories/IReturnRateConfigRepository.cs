using System.Threading.Tasks;
using Persistence.Entities;

namespace Persistence.Repositories
{
    public interface IReturnRateConfigRepository
    {
        Task<ReturnRateConfig?> GetConfigAsync();
        Task UpdateConfigAsync(ReturnRateConfig entity);
    }
}
