using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Application.Services;
using Application.ViewModels;

public class RankingSimulatorController : Controller
{
    private readonly IRankingService _rankingService;
    private readonly IMacroIndicatorService _macroService;
    private readonly ICountryIndicatorService _indicatorService;

    public RankingSimulatorController(IRankingService rankingService, IMacroIndicatorService macroService, ICountryIndicatorService indicatorService)
    {
        _rankingService = rankingService;
        _macroService = macroService;
        _indicatorService = indicatorService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var macros = await _macroService.GetAllAsync();
        var indicators = await _indicatorService.GetAllAsync();
        var years = indicators.Select(i => i.Year).Distinct().OrderByDescending(y => y).ToList();

        var viewModel = new RankingSimulatorViewModel
        {
            MacroIndicators = macros.Select(m => new MacroIndicatorSimConfigViewModel
            {
                MacroIndicatorId = m.Id,
                Name = m.Name,
                Weight = 0
            }).ToList(),
            Years = years,
            Results = new List<CountryRankingResult>()
        };

        ViewBag.YearOptions = years.Select(y => new SelectListItem { Value = y.ToString(), Text = y.ToString() }).ToList();

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Index(RankingSimulatorViewModel model)
    {
        var indicators = await _indicatorService.GetAllAsync();
        var years = indicators.Select(i => i.Year).Distinct().OrderByDescending(y => y).ToList();
        ViewBag.YearOptions = years.Select(y => new SelectListItem { Value = y.ToString(), Text = y.ToString() }).ToList();

        var simulationConfig = model.MacroIndicators
            .Where(m => m.Weight > 0)
            .Select(m => new MacroIndicatorSimulationConfig
            {
                MacroIndicatorId = m.MacroIndicatorId,
                Weight = m.Weight
            }).ToList();

        var totalWeight = simulationConfig.Sum(s => s.Weight);
        if (Math.Abs(totalWeight - 1m) > 0.0001m)
        {
            ModelState.AddModelError("", "La suma de los pesos de los macroindicadores debe ser exactamente 1.");
            model.Years = years;
            model.Results = new List<CountryRankingResult>();
            return View(model);
        }

        var results = await _rankingService.SimulateRankingAsync(model.SelectedYear, simulationConfig);
        model.Results = results;
        model.Years = years;
        return View(model);
    }
}
