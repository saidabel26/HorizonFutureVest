using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Persistence.Entities;
using Persistence.Repositories;

namespace Application.Services
{
    public class CountryIndicatorService : ICountryIndicatorService
    {
        private readonly ICountryIndicatorRepository _repository;

        public CountryIndicatorService(ICountryIndicatorRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CountryIndicatorDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            var dtos = new List<CountryIndicatorDto>();
            foreach (var entity in entities)
            {
                dtos.Add(new CountryIndicatorDto
                {
                    Id = entity.Id,
                    CountryId = entity.CountryId,
                    MacroIndicatorId = entity.MacroIndicatorId,
                    Value = entity.Value,
                    Year = entity.Year
                });
            }
            return dtos;
        }

        public async Task<CountryIndicatorDto?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;
            return new CountryIndicatorDto
            {
                Id = entity.Id,
                CountryId = entity.CountryId,
                MacroIndicatorId = entity.MacroIndicatorId,
                Value = entity.Value,
                Year = entity.Year
            };
        }

        public async Task AddAsync(CountryIndicatorDto dto)
        {
            // Validación de duplicados
            var allIndicators = await _repository.GetAllAsync();
            bool exists = allIndicators.Any(i =>
                i.CountryId == dto.CountryId &&
                i.MacroIndicatorId == dto.MacroIndicatorId &&
                i.Year == dto.Year);

            if (exists)
                throw new InvalidOperationException("Ya existe un indicador para este país, macroindicador y año.");

            var entity = new CountryIndicator
            {
                CountryId = dto.CountryId,
                MacroIndicatorId = dto.MacroIndicatorId,
                Value = dto.Value,
                Year = dto.Year
            };
            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(CountryIndicatorDto dto)
        {
            var existing = await _repository.GetByIdAsync(dto.Id);
            if (existing == null)
                throw new KeyNotFoundException("Indicador no encontrado.");

            existing.Value = dto.Value;
            await _repository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
