namespace NorwayPowderAlert.Models;

public class WeatherForecast
{
    public DateTime Time { get; set; }
    public double Temperature { get; set; }
    public double Precipitation { get; set; } // mm
    public double SnowfallMm { get; set; } // mm of snow
    public string Symbol { get; set; } = string.Empty;
}
