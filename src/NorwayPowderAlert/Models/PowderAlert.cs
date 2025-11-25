namespace NorwayPowderAlert.Models;

public class PowderAlert
{
    public SkiResort Resort { get; set; } = new();
    public double SnowfallCm24h { get; set; }
    public double SnowfallCm48h { get; set; }
    public double SnowfallCm3Day { get; set; }
    public double SnowfallCm7Day { get; set; }
    public double SnowfallCm10Day { get; set; }
    public double CurrentSnowDepthCm { get; set; }
    public double SeasonalSnowfallCm { get; set; }
    public DateTime ForecastTime { get; set; }
    public bool IsPowderDay { get; set; }
    public List<WeatherForecast> Forecast { get; set; } = new();
    public List<DailySnowfall> DailyBreakdown { get; set; } = new();
}
