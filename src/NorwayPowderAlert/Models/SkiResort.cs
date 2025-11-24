namespace NorwayPowderAlert.Models;

public class SkiResort
{
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Region { get; set; } = string.Empty;
    public int Elevation { get; set; } // meters
}
