using System.Collections.Generic;

namespace Application.ViewModels
{
    public class CountryIndicatorsIndexViewModel
    {
        public int? SelectedCountryId { get; set; }
        public int? YearFilter { get; set; }
        public List<int> CountryIds { get; set; } = new();
        public List<string> CountryNames { get; set; } = new();
        public List<CountryIndicatorListItem> Indicators { get; set; } = new();
    }

    public class CountryIndicatorListItem
    {
        public int Id { get; set; }
        public required string CountryName { get; set; }
        public required string MacroIndicatorName { get; set; }
        public decimal Value { get; set; }
        public int Year { get; set; }
    }
}
