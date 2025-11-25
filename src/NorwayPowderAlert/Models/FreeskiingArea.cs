namespace NorwayPowderAlert.Models;

public class FreeskiingArea
{
    public string Name { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Region { get; set; } = string.Empty;
    public int Elevation { get; set; } // meters
    public string Description { get; set; } = string.Empty;
    public string Difficulty { get; set; } = string.Empty; // Beginner, Intermediate, Advanced, Expert
    public string Terrain { get; set; } = string.Empty; // Powder bowls, Couloirs, Tree runs, etc.
    public string Access { get; set; } = string.Empty; // Lift-accessed, Ski touring, Heli-skiing
    public string ImageUrl { get; set; } = string.Empty;
    public bool AvalancheGearRequired { get; set; }
    public string BestMonths { get; set; } = string.Empty;
}
