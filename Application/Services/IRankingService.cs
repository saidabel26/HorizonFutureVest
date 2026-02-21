using System.Collections.Generic;
using System.Threading.Tasks;

public interface IRankingService
{
    Task<IList<CountryRankingResult>> GetCountryRankingAsync(int year);
    Task<IList<CountryRankingResult>> SimulateRankingAsync(int year, IList<MacroIndicatorSimulationConfig> simulationConfig);
}

public class CountryRankingResult
{
    public required string CountryName { get; set; }
    public required string IsoCode { get; set; }
    public decimal Scoring { get; set; }
    public decimal EstimatedReturnRate { get; set; }
}

public class MacroIndicatorSimulationConfig
{
    public int MacroIndicatorId { get; set; }
    public decimal Weight { get; set; }
}
