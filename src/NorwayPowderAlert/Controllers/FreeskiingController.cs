using Microsoft.AspNetCore.Mvc;
using NorwayPowderAlert.Services;
using NorwayPowderAlert.Models;

namespace NorwayPowderAlert.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FreeskiingController : ControllerBase
{
    private readonly FreeskiingService _freeskiingService;
    private readonly ILogger<FreeskiingController> _logger;

    public FreeskiingController(FreeskiingService freeskiingService, ILogger<FreeskiingController> logger)
    {
        _freeskiingService = freeskiingService;
        _logger = logger;
    }

    [HttpGet("areas")]
    public ActionResult<List<FreeskiingArea>> GetAllAreas()
    {
        try
        {
            var areas = _freeskiingService.GetAllAreas();
            return Ok(areas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching freeskiing areas");
            return StatusCode(500, "Error fetching freeskiing areas");
        }
    }

    [HttpGet("areas/{name}")]
    public ActionResult<FreeskiingArea> GetAreaByName(string name)
    {
        try
        {
            var area = _freeskiingService.GetAreaByName(name);
            if (area == null)
            {
                return NotFound($"Freeskiing area '{name}' not found");
            }
            return Ok(area);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching freeskiing area: {Name}", name);
            return StatusCode(500, "Error fetching freeskiing area");
        }
    }

    [HttpGet("areas/difficulty/{difficulty}")]
    public ActionResult<List<FreeskiingArea>> GetAreasByDifficulty(string difficulty)
    {
        try
        {
            var areas = _freeskiingService.GetAreasByDifficulty(difficulty);
            return Ok(areas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching areas by difficulty: {Difficulty}", difficulty);
            return StatusCode(500, "Error fetching areas by difficulty");
        }
    }
}
