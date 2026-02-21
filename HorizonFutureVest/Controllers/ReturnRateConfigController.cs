using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Application.ViewModels;

public class ReturnRateConfigController : Controller
{
    private readonly IReturnRateConfigService _configService;

    public ReturnRateConfigController(IReturnRateConfigService configService)
    {
        _configService = configService;
    }

    // Acción Index para mostrar y editar la configuración
    public async Task<IActionResult> Index()
    {
        var config = await _configService.GetConfigAsync();
        var model = new ReturnRateConfigViewModel
        {
            Id = config?.Id ?? 0,
            MinRate = config?.MinRate ?? 2,
            MaxRate = config?.MaxRate ?? 15
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Index(ReturnRateConfigViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await _configService.UpdateConfigAsync(new ReturnRateConfigDto
        {
            Id = model.Id,
            MinRate = model.MinRate,
            MaxRate = model.MaxRate
        });
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit()
    {
        var config = await _configService.GetConfigAsync();
        var model = new ReturnRateConfigViewModel
        {
            Id = config?.Id ?? 0,
            MinRate = config?.MinRate ?? 2,
            MaxRate = config?.MaxRate ?? 15
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ReturnRateConfigViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await _configService.UpdateConfigAsync(new ReturnRateConfigDto
        {
            Id = model.Id,
            MinRate = model.MinRate,
            MaxRate = model.MaxRate
        });
        return RedirectToAction("Edit");
    }
}
