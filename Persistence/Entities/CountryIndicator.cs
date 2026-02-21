using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities
{
    public class CountryIndicator
    {
        public int Id { get; set; }

        [Required]
        public int CountryId { get; set; }

        [Required]
        public int MacroIndicatorId { get; set; }

        [Required]
        public Country Country { get; set; } = null!; // Indica que nunca será nulo

        [Required]
        public MacroIndicator MacroIndicator { get; set; } = null!; // Indica que nunca será nulo

        [Required]
        public decimal Value { get; set; }

        [Required]
        public int Year { get; set; }
    }
}
