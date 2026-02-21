using System.Collections.Generic;

namespace Application.ViewModels
{
    public class RankingSimulatorViewModel
    {
        public int SelectedYear { get; set; }
        public List<int> Years { get; set; } = new();
        public List<MacroIndicatorSimConfigViewModel> MacroIndicators { get; set; } = new();
        public IList<CountryRankingResult> Results { get; set; } = new List<CountryRankingResult>();
    }

    public class MacroIndicatorSimConfigViewModel
    {
        public int MacroIndicatorId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Weight { get; set; }
    }
}
