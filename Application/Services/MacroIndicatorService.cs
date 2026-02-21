using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Persistence.Entities;
using Persistence.Repositories;

namespace Application.Services
{
    public class MacroIndicatorService : IMacroIndicatorService
    {
        private readonly IMacroIndicatorRepository _repository;

        public MacroIndicatorService(IMacroIndicatorRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<MacroIndicatorDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            var dtos = new List<MacroIndicatorDto>();
            foreach (var entity in entities)
            {
                dtos.Add(new MacroIndicatorDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Weight = entity.Weight,
                    IsHigherBetter = entity.IsHigherBetter
                });
            }
            return dtos;
        }

        public async Task<MacroIndicatorDto?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;
            return new MacroIndicatorDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Weight = entity.Weight,
                IsHigherBetter = entity.IsHigherBetter
            };
        }

        public async Task AddAsync(MacroIndicatorDto dto)
        {
            // Validación: nombre requerido
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new InvalidOperationException("El nombre del macroindicador es obligatorio.");

            // Validación: peso entre 0 y 1
            if (dto.Weight < 0 || dto.Weight > 1)
                throw new InvalidOperationException("El peso debe estar entre 0 y 1.");

            // Validación: suma de pesos no debe superar 1
            var allMacros = await _repository.GetAllAsync();
            var totalWeight = allMacros.Sum(m => m.Weight) + dto.Weight;
            if (totalWeight > 1)
                throw new InvalidOperationException("La suma de los pesos de los macroindicadores no puede superar 1.");

            // Validación: evitar duplicados por nombre
            bool exists = allMacros.Any(m => m.Name.Trim().ToLower() == dto.Name.Trim().ToLower());
            if (exists)
                throw new InvalidOperationException("Ya existe un macroindicador con ese nombre.");

            var entity = new MacroIndicator
            {
                Name = dto.Name,
                Weight = dto.Weight,
                IsHigherBetter = dto.IsHigherBetter
            };
            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(MacroIndicatorDto dto)
        {
            // Validación: nombre requerido
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new InvalidOperationException("El nombre del macroindicador es obligatorio.");

            // Validación: peso entre 0 y 1
            if (dto.Weight < 0 || dto.Weight > 1)
                throw new InvalidOperationException("El peso debe estar entre 0 y 1.");

            // Validación: suma de pesos no debe superar 1 (excepto el macroindicador actual)
            var allMacros = await _repository.GetAllAsync();
            var totalWeight = allMacros.Where(m => m.Id != dto.Id).Sum(m => m.Weight) + dto.Weight;
            if (totalWeight > 1)
                throw new InvalidOperationException("La suma de los pesos de los macroindicadores no puede superar 1.");

            // Validación: evitar duplicados por nombre (excepto el macroindicador actual)
            bool exists = allMacros.Any(m => m.Id != dto.Id && m.Name.Trim().ToLower() == dto.Name.Trim().ToLower());
            if (exists)
                throw new InvalidOperationException("Ya existe otro macroindicador con ese nombre.");

            var entity = new MacroIndicator
            {
                Id = dto.Id,
                Name = dto.Name,
                Weight = dto.Weight,
                IsHigherBetter = dto.IsHigherBetter
            };
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
