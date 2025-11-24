using NorwayPowderAlert.Models;

namespace NorwayPowderAlert.Services;

public class ResortService
{
    private readonly List<SkiResort> _resorts = new()
    {
        new SkiResort { Name = "Hemsedal", Latitude = 60.8630, Longitude = 8.5390, Region = "Buskerud", Elevation = 1450 },
        new SkiResort { Name = "Trysil", Latitude = 61.3157, Longitude = 12.2844, Region = "Innlandet", Elevation = 1132 },
        new SkiResort { Name = "Geilo", Latitude = 60.5342, Longitude = 8.2064, Region = "Buskerud", Elevation = 1178 },
        new SkiResort { Name = "Hafjell", Latitude = 61.2468, Longitude = 10.4342, Region = "Innlandet", Elevation = 1030 },
        new SkiResort { Name = "Kvitfjell", Latitude = 61.4558, Longitude = 10.1458, Region = "Innlandet", Elevation = 1030 },
        new SkiResort { Name = "Norefjell", Latitude = 60.1803, Longitude = 9.5444, Region = "Buskerud", Elevation = 1188 },
        new SkiResort { Name = "Myrkdalen", Latitude = 60.8833, Longitude = 6.8667, Region = "Vestland", Elevation = 1200 },
        new SkiResort { Name = "Voss", Latitude = 60.6272, Longitude = 6.4258, Region = "Vestland", Elevation = 945 },
        new SkiResort { Name = "Oppdal", Latitude = 62.5969, Longitude = 9.6844, Region = "Trøndelag", Elevation = 1050 },
        new SkiResort { Name = "Hovden", Latitude = 59.5583, Longitude = 7.3747, Region = "Agder", Elevation = 920 },
        new SkiResort { Name = "Skeikampen", Latitude = 61.1861, Longitude = 9.5992, Region = "Innlandet", Elevation = 1123 },
        new SkiResort { Name = "Kongsberg", Latitude = 59.6667, Longitude = 9.6500, Region = "Buskerud", Elevation = 600 },
        new SkiResort { Name = "Stranda", Latitude = 62.3167, Longitude = 6.9500, Region = "Møre og Romsdal", Elevation = 1035 },
        new SkiResort { Name = "Narvik", Latitude = 68.4333, Longitude = 17.4167, Region = "Nordland", Elevation = 1000 }
    };

    public List<SkiResort> GetAllResorts()
    {
        return _resorts;
    }

    public SkiResort? GetResortByName(string name)
    {
        return _resorts.FirstOrDefault(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public List<SkiResort> GetResortsByRegion(string region)
    {
        return _resorts.Where(r => r.Region.Equals(region, StringComparison.OrdinalIgnoreCase)).ToList();
    }
}
