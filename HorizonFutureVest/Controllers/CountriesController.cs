using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Application.ViewModels;

public class CountriesController : Controller
{
    private readonly ICountryService _countryService;

    public CountriesController(ICountryService countryService)
    {
        _countryService = countryService;
    }

    public async Task<IActionResult> Index()
    {
        var countries = await _countryService.GetAllAsync();
        var viewModels = countries.Select(c => new CountryViewModel
        {
            Id = c.Id,
            Name = c.Name,
            IsoCode = c.IsoCode
        }).ToList();
        return View(viewModels);
    }

    public IActionResult Create()
    {
        // Inicializa las propiedades required
        return View(new CountryViewModel { Name = string.Empty, IsoCode = string.Empty });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CountryViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            await _countryService.AddAsync(new CountryDto
            {
                Name = model.Name,
                IsoCode = model.IsoCode
            });
            return RedirectToAction("Index");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        var country = await _countryService.GetByIdAsync(id);
        if (country == null) return NotFound();

        var model = new CountryViewModel
        {
            Id = country.Id,
            Name = country.Name,
            IsoCode = country.IsoCode
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CountryViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            await _countryService.UpdateAsync(new CountryDto
            {
                Id = model.Id,
                Name = model.Name,
                IsoCode = model.IsoCode
            });
            return RedirectToAction("Index");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        var country = await _countryService.GetByIdAsync(id);
        if (country == null) return NotFound();

        var model = new CountryViewModel
        {
            Id = country.Id,
            Name = country.Name,
            IsoCode = country.IsoCode
        };
        return View(model);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _countryService.DeleteAsync(id);
        return RedirectToAction("Index");
    }
}
