using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Application.ViewModels
{
    public class ReturnRateConfigViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La tasa mínima es obligatoria.")]
        [Range(0, double.MaxValue, ErrorMessage = "Debe ser mayor o igual a 0.")]
        public decimal MinRate { get; set; }

        [Required(ErrorMessage = "La tasa máxima es obligatoria.")]
        [Range(0, double.MaxValue, ErrorMessage = "Debe ser mayor o igual a 0.")]
        public decimal MaxRate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MinRate >= MaxRate)
            {
                yield return new ValidationResult(
                    "La tasa mínima debe ser menor que la tasa máxima."
                );
            }
        }
    }
}
