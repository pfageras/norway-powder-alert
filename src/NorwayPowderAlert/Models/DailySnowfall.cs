namespace NorwayPowderAlert.Models;

public class DailySnowfall
{
    public DateTime Date { get; set; }
    public double SnowfallCm { get; set; }
    public double MinTemp { get; set; }
    public double MaxTemp { get; set; }
    public string Conditions { get; set; } = string.Empty;
}
