using Microsoft.AspNetCore.Mvc;
using NorwayPowderAlert.Services;

namespace NorwayPowderAlert.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PowderController : ControllerBase
{
    private readonly PowderAlertService _powderAlertService;
    private readonly ResortService _resortService;

    public PowderController(PowderAlertService powderAlertService, ResortService resortService)
    {
        _powderAlertService = powderAlertService;
        _resortService = resortService;
    }

    [HttpGet("alerts")]
    public async Task<IActionResult> GetAllAlerts()
    {
        var alerts = await _powderAlertService.GetAllPowderAlertsAsync();
        return Ok(alerts);
    }

    [HttpGet("powder-days")]
    public async Task<IActionResult> GetPowderDays()
    {
        var powderDays = await _powderAlertService.GetPowderDaysOnlyAsync();
        return Ok(powderDays);
    }

    [HttpGet("resort/{name}")]
    public async Task<IActionResult> GetResortAlert(string name)
    {
        var resort = _resortService.GetResortByName(name);
        if (resort == null)
        {
            return NotFound($"Resort '{name}' not found");
        }

        var alert = await _powderAlertService.GetPowderAlertForResortAsync(resort);
        return Ok(alert);
    }

    [HttpGet("resorts")]
    public IActionResult GetAllResorts()
    {
        var resorts = _resortService.GetAllResorts();
        return Ok(resorts);
    }
}
