using Application.Services;
using Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class HomeController : Controller
{
    private readonly IRankingService _rankingService;
    private readonly ICountryIndicatorService _indicatorService;

    public HomeController(IRankingService rankingService, ICountryIndicatorService indicatorService)
    {
        _rankingService = rankingService;
        _indicatorService = indicatorService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var indicators = await _indicatorService.GetAllAsync();
        var years = indicators.Select(i => i.Year).Distinct().OrderByDescending(y => y).ToList();

        ViewBag.YearOptions = years
            .Select(y => new SelectListItem
            {
                Value = y.ToString(),
                Text = y.ToString()
            })
            .Prepend(new SelectListItem
            {
                Value = "",
                Text = "Selecciona un año",
                Selected = true
            })
            .ToList();

        var viewModel = new HomeIndexViewModel
        {
            SelectedYear = null,
            Years = years,
            RankingResults = new List<CountryRankingResult>(),
            Message = string.Empty,
            LinkText = string.Empty,
            LinkUrl = string.Empty
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Index(HomeIndexViewModel model)
    {
        var indicators = await _indicatorService.GetAllAsync();
        var years = indicators.Select(i => i.Year).Distinct().OrderByDescending(y => y).ToList();

        ViewBag.YearOptions = years
            .Select(y => new SelectListItem
            {
                Value = y.ToString(),
                Text = y.ToString(),
                Selected = y == model.SelectedYear
            })
            .Prepend(new SelectListItem
            {
                Value = "",
                Text = "Selecciona un año",
                Selected = model.SelectedYear == null
            })
            .ToList();

        model.Years = years;

        if (!model.SelectedYear.HasValue)
        {
            model.Message = "Debes seleccionar un año para obtener el ranking.";
            model.RankingResults = new List<CountryRankingResult>();
            return View(model);
        }

        try
        {
            var results = await _rankingService.GetCountryRankingAsync(model.SelectedYear.Value);
            model.RankingResults = results;
            model.Message = string.Empty;
            model.LinkText = string.Empty;
            model.LinkUrl = string.Empty;
        }
        catch (InvalidOperationException ex)
        {
            model.RankingResults = new List<CountryRankingResult>();
            model.Message = ex.Message;
            if (ex.Message.Contains("macroindicadores"))
            {
                model.LinkText = "Ir a Macroindicadores";
                model.LinkUrl = "/MacroIndicators";
            }
            else if (ex.Message.Contains("suficientes países"))
            {
                model.LinkText = "Ir a Indicadores por Países";
                model.LinkUrl = "/CountryIndicators";
            }
            else
            {
                model.LinkText = string.Empty;
                model.LinkUrl = string.Empty;
            }
        }

        return View(model);
    }
}
