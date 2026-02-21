using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities
{
    public class ReturnRateConfig
    {
        public int Id { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal MinRate { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal MaxRate { get; set; }
    }
}
