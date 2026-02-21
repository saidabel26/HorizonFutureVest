using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Application.ViewModels;

public class MacroIndicatorsController : Controller
{
    private readonly IMacroIndicatorService _macroService;

    public MacroIndicatorsController(IMacroIndicatorService macroService)
    {
        _macroService = macroService;
    }

    public async Task<IActionResult> Index()
    {
        var macros = await _macroService.GetAllAsync();
        var viewModels = macros.Select(m => new MacroIndicatorViewModel
        {
            Id = m.Id,
            Name = m.Name,
            Weight = m.Weight,
            IsHigherBetter = m.IsHigherBetter
        }).ToList();
        return View(viewModels);
    }

    public IActionResult Create()
    {
        return View(new MacroIndicatorViewModel { Name = string.Empty });
    }

    [HttpPost]
    public async Task<IActionResult> Create(MacroIndicatorViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var macros = await _macroService.GetAllAsync();
        var sumaPesos = macros.Sum(m => m.Weight) + model.Weight;
        if (sumaPesos > 1m)
        {
            ModelState.AddModelError(string.Empty, "La suma de los pesos de los macroindicadores no puede superar 1.");
            return View(model);
        }

        try
        {
            await _macroService.AddAsync(new MacroIndicatorDto
            {
                Name = model.Name,
                Weight = model.Weight,
                IsHigherBetter = model.IsHigherBetter
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
        var macro = await _macroService.GetByIdAsync(id);
        if (macro == null) return NotFound();

        var model = new MacroIndicatorViewModel
        {
            Id = macro.Id,
            Name = macro.Name,
            Weight = macro.Weight,
            IsHigherBetter = macro.IsHigherBetter
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(MacroIndicatorViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var macros = await _macroService.GetAllAsync();
        var sumaPesos = macros.Where(m => m.Id != model.Id).Sum(m => m.Weight) + model.Weight;

        if (sumaPesos > 1m)
        {
            ModelState.AddModelError(string.Empty, "La suma de los pesos de los macroindicadores no puede superar 1.");
            return View(model);
        }

        try
        {
            await _macroService.UpdateAsync(new MacroIndicatorDto
            {
                Id = model.Id,
                Name = model.Name,
                Weight = model.Weight,
                IsHigherBetter = model.IsHigherBetter
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
        var macro = await _macroService.GetByIdAsync(id);
        if (macro == null) return NotFound();

        var model = new MacroIndicatorViewModel
        {
            Id = macro.Id,
            Name = macro.Name,
            Weight = macro.Weight,
            IsHigherBetter = macro.IsHigherBetter
        };
        return View(model);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _macroService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            var macro = await _macroService.GetByIdAsync(id);
            if (macro == null) return NotFound();

            var model = new MacroIndicatorViewModel
            {
                Id = macro.Id,
                Name = macro.Name,
                Weight = macro.Weight,
                IsHigherBetter = macro.IsHigherBetter
            };
            return View("Delete", model);
        }
    }
}
