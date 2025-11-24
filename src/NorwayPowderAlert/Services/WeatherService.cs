using System.Text.Json;
using NorwayPowderAlert.Models;

namespace NorwayPowderAlert.Services;

public class WeatherService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<WeatherService> _logger;

    public WeatherService(HttpClient httpClient, ILogger<WeatherService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;

        // Met.no requires a User-Agent header
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "NorwayPowderAlert/1.0 (+https://github.com/powder-alert)");
    }

    public async Task<List<WeatherForecast>> GetForecastAsync(double latitude, double longitude)
    {
        try
        {
            var url = $"https://api.met.no/weatherapi/locationforecast/2.0/compact?lat={latitude:F4}&lon={longitude:F4}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to fetch weather data: {StatusCode}", response.StatusCode);
                return new List<WeatherForecast>();
            }

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonDocument.Parse(json);

            var forecasts = new List<WeatherForecast>();
            var timeseries = data.RootElement.GetProperty("properties").GetProperty("timeseries");

            foreach (var entry in timeseries.EnumerateArray())
            {
                var time = entry.GetProperty("time").GetDateTime();
                var instant = entry.GetProperty("data").GetProperty("instant").GetProperty("details");

                var temperature = instant.GetProperty("air_temperature").GetDouble();

                // Get precipitation from next_1_hours if available
                double precipitation = 0;
                if (entry.GetProperty("data").TryGetProperty("next_1_hours", out var next1h))
                {
                    if (next1h.TryGetProperty("details", out var details))
                    {
                        if (details.TryGetProperty("precipitation_amount", out var precip))
                        {
                            precipitation = precip.GetDouble();
                        }
                    }
                }

                // Estimate snowfall based on temperature (snow if temp < 1°C)
                double snowfall = temperature < 1.0 ? precipitation : 0;

                var forecast = new WeatherForecast
                {
                    Time = time,
                    Temperature = temperature,
                    Precipitation = precipitation,
                    SnowfallMm = snowfall,
                    Symbol = GetSymbol(entry)
                };

                forecasts.Add(forecast);
            }

            return forecasts;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching weather data for lat={Lat}, lon={Lon}", latitude, longitude);
            return new List<WeatherForecast>();
        }
    }

    private string GetSymbol(JsonElement entry)
    {
        try
        {
            if (entry.GetProperty("data").TryGetProperty("next_1_hours", out var next1h))
            {
                if (next1h.TryGetProperty("summary", out var summary))
                {
                    if (summary.TryGetProperty("symbol_code", out var symbol))
                    {
                        return symbol.GetString() ?? "unknown";
                    }
                }
            }
            return "unknown";
        }
        catch
        {
            return "unknown";
        }
    }

    public double CalculateSnowfall24h(List<WeatherForecast> forecasts, DateTime fromTime)
    {
        var endTime = fromTime.AddHours(24);
        var snowInPeriod = forecasts
            .Where(f => f.Time >= fromTime && f.Time < endTime)
            .Sum(f => f.SnowfallMm);

        // Convert mm to cm (rough approximation: 10mm water = ~10cm snow)
        // More accurate would be 1mm water = 10cm snow for light powder
        return snowInPeriod * 1.0; // This gives us cm directly (conservative estimate)
    }

    public double CalculateSnowfall48h(List<WeatherForecast> forecasts, DateTime fromTime)
    {
        var endTime = fromTime.AddHours(48);
        var snowInPeriod = forecasts
            .Where(f => f.Time >= fromTime && f.Time < endTime)
            .Sum(f => f.SnowfallMm);

        return snowInPeriod * 1.0; // cm
    }

    public double CalculateSnowfall3Day(List<WeatherForecast> forecasts, DateTime fromTime)
    {
        var endTime = fromTime.AddDays(3);
        var snowInPeriod = forecasts
            .Where(f => f.Time >= fromTime && f.Time < endTime)
            .Sum(f => f.SnowfallMm);

        return snowInPeriod * 1.0; // cm
    }

    public double CalculateSnowfall7Day(List<WeatherForecast> forecasts, DateTime fromTime)
    {
        var endTime = fromTime.AddDays(7);
        var snowInPeriod = forecasts
            .Where(f => f.Time >= fromTime && f.Time < endTime)
            .Sum(f => f.SnowfallMm);

        return snowInPeriod * 1.0; // cm
    }

    public double CalculateSnowfall10Day(List<WeatherForecast> forecasts, DateTime fromTime)
    {
        var endTime = fromTime.AddDays(10);
        var snowInPeriod = forecasts
            .Where(f => f.Time >= fromTime && f.Time < endTime)
            .Sum(f => f.SnowfallMm);

        return snowInPeriod * 1.0; // cm
    }

    public List<DailySnowfall> GetDailyBreakdown(List<WeatherForecast> forecasts, DateTime fromTime, int days = 10)
    {
        var dailyBreakdown = new List<DailySnowfall>();

        for (int i = 0; i < days; i++)
        {
            var dayStart = fromTime.AddDays(i).Date;
            var dayEnd = dayStart.AddDays(1);

            var dayForecasts = forecasts
                .Where(f => f.Time >= dayStart && f.Time < dayEnd)
                .ToList();

            if (dayForecasts.Any())
            {
                var snowfall = dayForecasts.Sum(f => f.SnowfallMm) * 1.0; // cm
                var minTemp = dayForecasts.Min(f => f.Temperature);
                var maxTemp = dayForecasts.Max(f => f.Temperature);
                var conditions = DetermineConditions(dayForecasts);

                dailyBreakdown.Add(new DailySnowfall
                {
                    Date = dayStart,
                    SnowfallCm = Math.Round(snowfall, 1),
                    MinTemp = Math.Round(minTemp, 1),
                    MaxTemp = Math.Round(maxTemp, 1),
                    Conditions = conditions
                });
            }
        }

        return dailyBreakdown;
    }

    private string DetermineConditions(List<WeatherForecast> dayForecasts)
    {
        var totalSnow = dayForecasts.Sum(f => f.SnowfallMm);
        var avgTemp = dayForecasts.Average(f => f.Temperature);

        if (totalSnow >= 20) return "Heavy Snow";
        if (totalSnow >= 10) return "Moderate Snow";
        if (totalSnow >= 5) return "Light Snow";
        if (totalSnow > 0) return "Flurries";
        if (avgTemp < 0) return "Cold & Clear";
        return "Partly Cloudy";
    }
}
