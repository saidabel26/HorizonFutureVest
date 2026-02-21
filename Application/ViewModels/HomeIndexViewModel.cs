using System.Collections.Generic;

namespace Application.ViewModels
{
    public class HomeIndexViewModel
    {
        public int? SelectedYear { get; set; }
        public List<int> Years { get; set; } = new();
        public IList<CountryRankingResult> RankingResults { get; set; } = new List<CountryRankingResult>();
        public string Message { get; set; } = string.Empty;
        public string LinkText { get; set; } = string.Empty;
        public string LinkUrl { get; set; } = string.Empty;
    }
}
