using NorwayPowderAlert.Models;

namespace NorwayPowderAlert.Services;

public class PowderAlertService
{
    private readonly WeatherService _weatherService;
    private readonly ResortService _resortService;
    private readonly double _powderThresholdCm = 20.0; // 20cm+ in 24h

    public PowderAlertService(WeatherService weatherService, ResortService resortService)
    {
        _weatherService = weatherService;
        _resortService = resortService;
    }

    public async Task<List<PowderAlert>> GetAllPowderAlertsAsync()
    {
        var resorts = _resortService.GetAllResorts();
        var alerts = new List<PowderAlert>();

        foreach (var resort in resorts)
        {
            var alert = await GetPowderAlertForResortAsync(resort);
            alerts.Add(alert);
        }

        return alerts.OrderByDescending(a => a.SnowfallCm24h).ToList();
    }

    public async Task<PowderAlert> GetPowderAlertForResortAsync(SkiResort resort)
    {
        var forecasts = await _weatherService.GetForecastAsync(resort.Latitude, resort.Longitude);
        var now = DateTime.UtcNow;

        var snowfall24h = _weatherService.CalculateSnowfall24h(forecasts, now);
        var snowfall48h = _weatherService.CalculateSnowfall48h(forecasts, now);
        var snowfall3Day = _weatherService.CalculateSnowfall3Day(forecasts, now);
        var snowfall7Day = _weatherService.CalculateSnowfall7Day(forecasts, now);
        var snowfall10Day = _weatherService.CalculateSnowfall10Day(forecasts, now);
        var dailyBreakdown = _weatherService.GetDailyBreakdown(forecasts, now, 10);

        return new PowderAlert
        {
            Resort = resort,
            SnowfallCm24h = Math.Round(snowfall24h, 1),
            SnowfallCm48h = Math.Round(snowfall48h, 1),
            SnowfallCm3Day = Math.Round(snowfall3Day, 1),
            SnowfallCm7Day = Math.Round(snowfall7Day, 1),
            SnowfallCm10Day = Math.Round(snowfall10Day, 1),
            ForecastTime = now,
            IsPowderDay = snowfall24h >= _powderThresholdCm,
            Forecast = forecasts.Take(48).ToList(), // Next 48 hours of detailed forecast
            DailyBreakdown = dailyBreakdown
        };
    }

    public async Task<List<PowderAlert>> GetPowderDaysOnlyAsync()
    {
        var allAlerts = await GetAllPowderAlertsAsync();
        return allAlerts.Where(a => a.IsPowderDay).ToList();
    }
}
