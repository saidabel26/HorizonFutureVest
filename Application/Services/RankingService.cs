using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistence.Entities;
using Persistence.Repositories;

public class RankingService : IRankingService
{
    private readonly ICountryRepository _countryRepo;
    private readonly IMacroIndicatorRepository _macroRepo;
    private readonly ICountryIndicatorRepository _indicatorRepo;
    private readonly IReturnRateConfigRepository _rateConfigRepo;

    public RankingService(
        ICountryRepository countryRepo,
        IMacroIndicatorRepository macroRepo,
        ICountryIndicatorRepository indicatorRepo,
        IReturnRateConfigRepository rateConfigRepo)
    {
        _countryRepo = countryRepo;
        _macroRepo = macroRepo;
        _indicatorRepo = indicatorRepo;
        _rateConfigRepo = rateConfigRepo;
    }

    public async Task<IList<CountryRankingResult>> GetCountryRankingAsync(int year)
    {
        var macroIndicators = (await _macroRepo.GetAllAsync()).Where(m => m.Weight > 0).ToList();
        var countries = await _countryRepo.GetAllAsync();
        var indicators = await _indicatorRepo.GetAllAsync();
        var rateConfig = await _rateConfigRepo.GetConfigAsync();

        decimal totalWeight = macroIndicators.Sum(m => m.Weight);
        if (totalWeight != 1)
            throw new InvalidOperationException("La suma de los pesos de los macroindicadores debe ser igual a 1.");

        var eligibleCountries = countries.Where(country =>
            macroIndicators.All(macro =>
                indicators.Any(ind => ind.CountryId == country.Id && ind.MacroIndicatorId == macro.Id && ind.Year == year)
            )
        ).ToList();

        if (eligibleCountries.Count < 2)
            throw new InvalidOperationException("No hay suficientes países elegibles para calcular el ranking.");

        var results = new List<CountryRankingResult>();

        foreach (var country in eligibleCountries)
        {
            decimal scoring = 0;
            foreach (var macro in macroIndicators)
            {
                var countryIndicator = indicators.First(ind => ind.CountryId == country.Id && ind.MacroIndicatorId == macro.Id && ind.Year == year);
                var values = indicators
                    .Where(ind => ind.MacroIndicatorId == macro.Id && ind.Year == year && eligibleCountries.Any(c => c.Id == ind.CountryId))
                    .Select(ind => ind.Value)
                    .ToList();

                decimal min = values.Min();
                decimal max = values.Max();
                decimal norm = 0.5m;

                if (max != min)
                {
                    if (macro.IsHigherBetter)
                        norm = (countryIndicator.Value - min) / (max - min);
                    else
                        norm = (max - countryIndicator.Value) / (max - min);
                }

                decimal subScore = norm * macro.Weight;
                scoring += subScore;
            }

            scoring = Math.Clamp(scoring, 0, 1);

            decimal minRate = rateConfig?.MinRate > 0 ? rateConfig.MinRate : 2;
            decimal maxRate = rateConfig?.MaxRate > 0 ? rateConfig.MaxRate : 15;
            decimal estimatedReturn = minRate + (maxRate - minRate) * scoring;

            results.Add(new CountryRankingResult
            {
                CountryName = country.Name,
                IsoCode = country.IsoCode,
                Scoring = Math.Round(scoring, 2),
                EstimatedReturnRate = Math.Round(estimatedReturn, 2)
            });
        }

        return results.OrderByDescending(r => r.Scoring).ToList();
    }

    public async Task<IList<CountryRankingResult>> SimulateRankingAsync(int year, IList<MacroIndicatorSimulationConfig> simulationConfig)
    {
        var macroIndicators = (await _macroRepo.GetAllAsync())
            .Where(m => simulationConfig.Any(s => s.MacroIndicatorId == m.Id && s.Weight > 0))
            .ToList();

        decimal totalWeight = simulationConfig.Sum(s => s.Weight);
        if (totalWeight != 1)
            throw new InvalidOperationException("La suma de los pesos de los macroindicadores de la simulación debe ser igual a 1.");

        var countries = await _countryRepo.GetAllAsync();
        var indicators = await _indicatorRepo.GetAllAsync();
        var rateConfig = await _rateConfigRepo.GetConfigAsync();

        var eligibleCountries = countries.Where(country =>
            simulationConfig.All(sim =>
                indicators.Any(ind => ind.CountryId == country.Id && ind.MacroIndicatorId == sim.MacroIndicatorId && ind.Year == year)
            )
        ).ToList();

        if (eligibleCountries.Count < 2)
            throw new InvalidOperationException("No hay suficientes países elegibles para la simulación.");

        var results = new List<CountryRankingResult>();

        foreach (var country in eligibleCountries)
        {
            decimal scoring = 0;
            foreach (var sim in simulationConfig)
            {
                var macro = macroIndicators.First(m => m.Id == sim.MacroIndicatorId);
                var countryIndicator = indicators.First(ind => ind.CountryId == country.Id && ind.MacroIndicatorId == macro.Id && ind.Year == year);
                var values = indicators
                    .Where(ind => ind.MacroIndicatorId == macro.Id && ind.Year == year && eligibleCountries.Any(c => c.Id == ind.CountryId))
                    .Select(ind => ind.Value)
                    .ToList();

                decimal min = values.Min();
                decimal max = values.Max();
                decimal norm = 0.5m;

                if (max != min)
                {
                    if (macro.IsHigherBetter)
                        norm = (countryIndicator.Value - min) / (max - min);
                    else
                        norm = (max - countryIndicator.Value) / (max - min);
                }

                decimal subScore = norm * sim.Weight;
                scoring += subScore;
            }

            scoring = Math.Clamp(scoring, 0, 1);

            decimal minRate = rateConfig?.MinRate > 0 ? rateConfig.MinRate : 2;
            decimal maxRate = rateConfig?.MaxRate > 0 ? rateConfig.MaxRate : 15;
            decimal estimatedReturn = minRate + (maxRate - minRate) * scoring;

            results.Add(new CountryRankingResult
            {
                CountryName = country.Name,
                IsoCode = country.IsoCode,
                Scoring = Math.Round(scoring, 2),
                EstimatedReturnRate = Math.Round(estimatedReturn, 2)
            });
        }

        return results.OrderByDescending(r => r.Scoring).ToList();
    }
}
