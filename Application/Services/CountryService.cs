using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Persistence.Entities;
using Persistence.Repositories;

namespace Application.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _repository;

        public CountryService(ICountryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CountryDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            var dtos = new List<CountryDto>();
            foreach (var entity in entities)
            {
                dtos.Add(new CountryDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    IsoCode = entity.IsoCode
                });
            }
            return dtos;
        }

        public async Task<CountryDto?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;
            return new CountryDto
            {
                Id = entity.Id,
                Name = entity.Name,
                IsoCode = entity.IsoCode
            };
        }

        public async Task AddAsync(CountryDto dto)
        {
            // Validación: nombre y código ISO requeridos
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new InvalidOperationException("El nombre del país es obligatorio.");
            if (string.IsNullOrWhiteSpace(dto.IsoCode))
                throw new InvalidOperationException("El código ISO es obligatorio.");

            // Validación: evitar duplicados por nombre o código ISO
            var allCountries = await _repository.GetAllAsync();
            bool exists = allCountries.Any(c =>
                c.Name.Trim().ToLower() == dto.Name.Trim().ToLower() ||
                c.IsoCode.Trim().ToUpper() == dto.IsoCode.Trim().ToUpper());

            if (exists)
                throw new InvalidOperationException("Ya existe un país con ese nombre o código ISO.");

            var entity = new Country
            {
                Name = dto.Name,
                IsoCode = dto.IsoCode
            };
            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(CountryDto dto)
        {
            // Validación: nombre y código ISO requeridos
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new InvalidOperationException("El nombre del país es obligatorio.");
            if (string.IsNullOrWhiteSpace(dto.IsoCode))
                throw new InvalidOperationException("El código ISO es obligatorio.");

            // Validación: evitar duplicados por nombre o código ISO (excepto el país actual)
            var allCountries = await _repository.GetAllAsync();
            bool exists = allCountries.Any(c =>
                c.Id != dto.Id &&
                (c.Name.Trim().ToLower() == dto.Name.Trim().ToLower() ||
                 c.IsoCode.Trim().ToUpper() == dto.IsoCode.Trim().ToUpper()));

            if (exists)
                throw new InvalidOperationException("Ya existe otro país con ese nombre o código ISO.");

            var entity = new Country
            {
                Id = dto.Id,
                Name = dto.Name,
                IsoCode = dto.IsoCode
            };
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
