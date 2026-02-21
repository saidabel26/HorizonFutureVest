using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels
{
    public class CountryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "El código ISO es obligatorio.")]
        [MaxLength(3)]
        public required string IsoCode { get; set; }
    }
}
