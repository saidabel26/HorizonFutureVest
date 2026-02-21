using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels
{
    public class CountryIndicatorViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El país es obligatorio.")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "El macroindicador es obligatorio.")]
        public int MacroIndicatorId { get; set; }

        [Required(ErrorMessage = "El valor es obligatorio.")]
        public decimal Value { get; set; }

        [Required(ErrorMessage = "El año es obligatorio.")]
        public int Year { get; set; }
        public string CountryName { get; set; } = string.Empty;
        public string MacroIndicatorName { get; set; } = string.Empty;
    }
}
