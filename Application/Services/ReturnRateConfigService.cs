using System.Threading.Tasks;
using Persistence.Entities;
using Persistence.Repositories;

namespace Application.Services
{
    public class ReturnRateConfigService : IReturnRateConfigService
    {
        private readonly IReturnRateConfigRepository _repository;

        public ReturnRateConfigService(IReturnRateConfigRepository repository)
        {
            _repository = repository;
        }

        public async Task<ReturnRateConfigDto?> GetConfigAsync()
        {
            var entity = await _repository.GetConfigAsync();
            if (entity == null) return null;
            return new ReturnRateConfigDto
            {
                Id = entity.Id,
                MinRate = entity.MinRate,
                MaxRate = entity.MaxRate
            };
        }

        public async Task UpdateConfigAsync(ReturnRateConfigDto dto)
        {
            var entity = new ReturnRateConfig
            {
                Id = dto.Id,
                MinRate = dto.MinRate,
                MaxRate = dto.MaxRate
            };
            await _repository.UpdateConfigAsync(entity);
        }
    }
}
