using System.Text.Json;
using NorwayPowderAlert.Models;

namespace NorwayPowderAlert.Services;

public class SnowDepthService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SnowDepthService> _logger;
    private const string FrostApiBase = "https://frost.met.no/observations/v0.jsonld";

    public SnowDepthService(HttpClient httpClient, ILogger<SnowDepthService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<double> GetSnowDepthAsync(double latitude, double longitude, int elevation)
    {
        // Note: Frost API requires authentication (client ID) which is not configured
        // For now, we estimate snow depth based on elevation and time of season
        // TODO: Configure Frost API authentication for real snow depth data

        try
        {
            // Estimate base snow depth by elevation
            var baseDepth = elevation switch
            {
                >= 1400 => 100.0,  // High elevation resorts
                >= 1200 => 85.0,
                >= 1000 => 70.0,
                >= 800 => 55.0,
                _ => 40.0          // Lower elevation resorts
            };

            // Add seasonal variation (mid-winter has most snow)
            var now = DateTime.UtcNow;
            var monthFactor = now.Month switch
            {
                12 or 1 => 0.8,    // Early season
                2 or 3 => 1.0,     // Peak season
                4 => 0.7,          // Late season
                _ => 0.5           // Off season
            };

            var estimatedDepth = baseDepth * monthFactor;

            _logger.LogInformation("Estimated snow depth for elevation {Elevation}m: {Depth}cm",
                elevation, Math.Round(estimatedDepth, 0));

            return Math.Round(estimatedDepth, 0);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error estimating snow depth for lat={Lat}, lon={Lon}, elevation={Elevation}",
                latitude, longitude, elevation);
            return 0;
        }
    }

    public double EstimateSeasonalSnowfall(int elevation)
    {
        // Estimate total seasonal snowfall based on elevation
        // This is a rough approximation for Norwegian resorts
        return elevation switch
        {
            >= 1400 => 450.0,  // High elevation: ~450cm season total
            >= 1200 => 400.0,
            >= 1000 => 350.0,
            >= 800 => 300.0,
            _ => 250.0
        };
    }
}
