using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities
{
    public class MacroIndicator
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        [Range(0, 1)]
        public decimal Weight { get; set; }

        [Required]
        public bool IsHigherBetter { get; set; }

        public ICollection<CountryIndicator> CountryIndicators { get; set; } = new List<CountryIndicator>();
    }
}
