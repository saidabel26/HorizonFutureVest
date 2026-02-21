using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Application.Services;
using Application.ViewModels;

public class CountryIndicatorsController : Controller
{
    private readonly ICountryIndicatorService _indicatorService;
    private readonly ICountryService _countryService;
    private readonly IMacroIndicatorService _macroService;

    public CountryIndicatorsController(ICountryIndicatorService indicatorService, ICountryService countryService, IMacroIndicatorService macroService)
    {
        _indicatorService = indicatorService;
        _countryService = countryService;
        _macroService = macroService;
    }

    public async Task<IActionResult> Index(int? SelectedCountryId, int? YearFilter)
    {
        var countries = await _countryService.GetAllAsync();
        var macros = await _macroService.GetAllAsync();
        var indicators = await _indicatorService.GetAllAsync();

        var filtered = indicators.Where(i =>
            (!SelectedCountryId.HasValue || i.CountryId == SelectedCountryId) &&
            (!YearFilter.HasValue || i.Year == YearFilter)
        ).ToList();

        var viewModel = new CountryIndicatorsIndexViewModel
        {
            CountryIds = countries.Select(c => c.Id).ToList(),
            CountryNames = countries.Select(c => c.Name).ToList(),
            Indicators = filtered.Select(i => new CountryIndicatorListItem
            {
                Id = i.Id,
                CountryName = countries.First(c => c.Id == i.CountryId).Name,
                MacroIndicatorName = macros.First(m => m.Id == i.MacroIndicatorId).Name,
                Value = i.Value,
                Year = i.Year
            }).ToList(),
            SelectedCountryId = SelectedCountryId,
            YearFilter = YearFilter
        };

        ViewBag.CountryOptions = countries.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
        ViewBag.YearOptions = indicators
            .Select(i => i.Year)
            .Distinct()
            .OrderByDescending(y => y)
            .Select(y => new SelectListItem { Value = y.ToString(), Text = y.ToString() })
            .ToList();

        return View(viewModel);
    }

    public async Task<IActionResult> Create()
    {
        var countries = await _countryService.GetAllAsync();
        var macros = await _macroService.GetAllAsync();
        ViewBag.CountryOptions = countries.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
        ViewBag.MacroIndicatorOptions = macros.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
        return View(new CountryIndicatorViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CountryIndicatorViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var countries = await _countryService.GetAllAsync();
            var macros = await _macroService.GetAllAsync();
            ViewBag.CountryOptions = countries.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
            ViewBag.MacroIndicatorOptions = macros.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
            return View(model);
        }

        try
        {
            await _indicatorService.AddAsync(new CountryIndicatorDto
            {
                CountryId = model.CountryId,
                MacroIndicatorId = model.MacroIndicatorId,
                Value = model.Value,
                Year = model.Year
            });
            return RedirectToAction("Index");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            var countries = await _countryService.GetAllAsync();
            var macros = await _macroService.GetAllAsync();
            ViewBag.CountryOptions = countries.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
            ViewBag.MacroIndicatorOptions = macros.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
            return View(model);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        var indicator = await _indicatorService.GetByIdAsync(id);
        if (indicator == null) return NotFound();

        var countries = await _countryService.GetAllAsync();
        var macros = await _macroService.GetAllAsync();
        ViewBag.CountryOptions = countries.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
        ViewBag.MacroIndicatorOptions = macros.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();

        var model = new CountryIndicatorViewModel
        {
            Id = indicator.Id,
            CountryId = indicator.CountryId,
            MacroIndicatorId = indicator.MacroIndicatorId,
            Value = indicator.Value,
            Year = indicator.Year
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CountryIndicatorViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var countries = await _countryService.GetAllAsync();
            var macros = await _macroService.GetAllAsync();
            ViewBag.CountryOptions = countries.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
            ViewBag.MacroIndicatorOptions = macros.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
            return View(model);
        }

        try
        {
            await _indicatorService.UpdateAsync(new CountryIndicatorDto
            {
                Id = model.Id,
                CountryId = model.CountryId,
                MacroIndicatorId = model.MacroIndicatorId,
                Value = model.Value,
                Year = model.Year
            });
            return RedirectToAction("Index");
        }
        catch (KeyNotFoundException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            var countries = await _countryService.GetAllAsync();
            var macros = await _macroService.GetAllAsync();
            ViewBag.CountryOptions = countries.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name }).ToList();
            ViewBag.MacroIndicatorOptions = macros.Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Name }).ToList();
            return View(model);
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        var indicator = await _indicatorService.GetByIdAsync(id);
        if (indicator == null) return NotFound();

        // Obtener nombres usando los servicios
        var country = await _countryService.GetByIdAsync(indicator.CountryId);
        var macro = await _macroService.GetByIdAsync(indicator.MacroIndicatorId);

        var model = new CountryIndicatorViewModel
        {
            Id = indicator.Id,
            CountryId = indicator.CountryId,
            MacroIndicatorId = indicator.MacroIndicatorId,
            Value = indicator.Value,
            Year = indicator.Year,
            CountryName = country?.Name ?? "",
            MacroIndicatorName = macro?.Name ?? ""
        };
        return View(model);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _indicatorService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
        catch (KeyNotFoundException ex)
        {
            // Si el indicador no existe, muestra el mensaje en la vista de eliminación
            ModelState.AddModelError(string.Empty, ex.Message);
            var indicator = await _indicatorService.GetByIdAsync(id);
            if (indicator == null)
            {
                // Si no se encuentra, redirige al listado
                return RedirectToAction("Index");
            }
            var country = await _countryService.GetByIdAsync(indicator.CountryId);
            var macro = await _macroService.GetByIdAsync(indicator.MacroIndicatorId);
            var model = new CountryIndicatorViewModel
            {
                Id = indicator.Id,
                CountryId = indicator.CountryId,
                MacroIndicatorId = indicator.MacroIndicatorId,
                Value = indicator.Value,
                Year = indicator.Year,
                CountryName = country?.Name ?? "",
                MacroIndicatorName = macro?.Name ?? ""
            };
            return View("Delete", model);
        }
    }
}
