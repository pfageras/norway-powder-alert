using System.Text;
using System.Text.Json;
using NorwayPowderAlert.Models;

namespace NorwayPowderAlert.Services;

public class SnowDepthService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SnowDepthService> _logger;
    private readonly IConfiguration _configuration;
    private const string FrostApiBase = "https://frost.met.no/observations/v0.jsonld";

    public SnowDepthService(HttpClient httpClient, ILogger<SnowDepthService> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<double> GetSnowDepthAsync(double latitude, double longitude)
    {
        try
        {
            var clientId = _configuration["FrostApi:ClientId"];
            if (string.IsNullOrEmpty(clientId))
            {
                _logger.LogWarning("Frost API client ID not configured");
                return 0;
            }

            // Find nearest weather station with snow depth data
            var now = DateTime.UtcNow;
            var yesterday = now.AddDays(-1);

            // Frost API query for snow depth (surface_snow_thickness)
            // We search for stations near the coordinates
            var url = $"{FrostApiBase}?" +
                      $"nearestmaxcount=1&" +
                      $"geometry=nearest(POINT({longitude:F4} {latitude:F4}))&" +
                      $"referencetime={yesterday:yyyy-MM-dd}/{now:yyyy-MM-dd}&" +
                      $"elements=surface_snow_thickness";

            // Set up Basic Authentication with client ID
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var authBytes = Encoding.ASCII.GetBytes($"{clientId}:");
            var authHeader = Convert.ToBase64String(authBytes);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authHeader);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to fetch snow depth data: {StatusCode}", response.StatusCode);
                return 0; // Return 0 if data not available
            }

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonDocument.Parse(json);

            // Parse the response to get the latest snow depth observation
            if (data.RootElement.TryGetProperty("data", out var dataArray))
            {
                foreach (var observation in dataArray.EnumerateArray())
                {
                    if (observation.TryGetProperty("observations", out var observations))
                    {
                        foreach (var obs in observations.EnumerateArray())
                        {
                            if (obs.TryGetProperty("value", out var value))
                            {
                                var depthMeters = value.GetDouble();
                                return Math.Round(depthMeters * 100, 0); // Convert meters to cm
                            }
                        }
                    }
                }
            }

            return 0; // No data found
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching snow depth data for lat={Lat}, lon={Lon}", latitude, longitude);
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
