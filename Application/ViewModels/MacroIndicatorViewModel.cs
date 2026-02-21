using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels
{
    public class MacroIndicatorViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "El peso es obligatorio.")]
        [Range(0, 1, ErrorMessage = "El peso debe estar entre 0 y 1.")]
        public decimal Weight { get; set; }

        [Required(ErrorMessage = "Debe indicar si es mejor más alto.")]
        public bool IsHigherBetter { get; set; }
    }
}
