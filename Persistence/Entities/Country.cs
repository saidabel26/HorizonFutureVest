using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities
{
    public class Country
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(3)]
        public required string IsoCode { get; set; }

        public ICollection<CountryIndicator> Indicators { get; set; } = new List<CountryIndicator>();
    }
}
