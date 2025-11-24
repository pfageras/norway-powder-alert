using NorwayPowderAlert.Models;

namespace NorwayPowderAlert.Services;

public class ResortService
{
    private readonly List<SkiResort> _resorts = new()
    {
        new SkiResort { Name = "Hemsedal", Latitude = 60.8630, Longitude = 8.5390, Region = "Buskerud", Elevation = 1450, ImageUrl = "https://images.unsplash.com/photo-1605540436563-5bca919ae766?w=800&h=600&fit=crop" },
        new SkiResort { Name = "Trysil", Latitude = 61.3157, Longitude = 12.2844, Region = "Innlandet", Elevation = 1132, ImageUrl = "https://images.unsplash.com/photo-1551524164-687a55dd1126?w=800&h=600&fit=crop" },
        new SkiResort { Name = "Geilo", Latitude = 60.5342, Longitude = 8.2064, Region = "Buskerud", Elevation = 1178, ImageUrl = "https://images.unsplash.com/photo-1545572695-7f27679e47c2?w=800&h=600&fit=crop" },
        new SkiResort { Name = "Hafjell", Latitude = 61.2468, Longitude = 10.4342, Region = "Innlandet", Elevation = 1030, ImageUrl = "https://images.unsplash.com/photo-1609137144813-7d9921338f24?w=800&h=600&fit=crop" },
        new SkiResort { Name = "Kvitfjell", Latitude = 61.4558, Longitude = 10.1458, Region = "Innlandet", Elevation = 1030, ImageUrl = "https://images.unsplash.com/photo-1551524164-687a55dd1126?w=800&h=600&fit=crop&sat=-10" },
        new SkiResort { Name = "Norefjell", Latitude = 60.1803, Longitude = 9.5444, Region = "Buskerud", Elevation = 1188, ImageUrl = "https://images.unsplash.com/photo-1544986581-efac024faf62?w=800&h=600&fit=crop" },
        new SkiResort { Name = "Myrkdalen", Latitude = 60.8833, Longitude = 6.8667, Region = "Vestland", Elevation = 1200, ImageUrl = "https://images.unsplash.com/photo-1519904981063-b0cf448d479e?w=800&h=600&fit=crop" },
        new SkiResort { Name = "Voss", Latitude = 60.6272, Longitude = 6.4258, Region = "Vestland", Elevation = 945, ImageUrl = "https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?w=800&h=600&fit=crop" },
        new SkiResort { Name = "Oppdal", Latitude = 62.5969, Longitude = 9.6844, Region = "Trøndelag", Elevation = 1050, ImageUrl = "https://images.unsplash.com/photo-1548704888-e2e6cc6c81ed?w=800&h=600&fit=crop" },
        new SkiResort { Name = "Hovden", Latitude = 59.5583, Longitude = 7.3747, Region = "Agder", Elevation = 920, ImageUrl = "https://images.unsplash.com/photo-1605540436563-5bca919ae766?w=800&h=600&fit=crop&sat=-5" },
        new SkiResort { Name = "Skeikampen", Latitude = 61.1861, Longitude = 9.5992, Region = "Innlandet", Elevation = 1123, ImageUrl = "https://images.unsplash.com/photo-1542728928-1413d1894ed1?w=800&h=600&fit=crop" },
        new SkiResort { Name = "Kongsberg", Latitude = 59.6667, Longitude = 9.6500, Region = "Buskerud", Elevation = 600, ImageUrl = "https://images.unsplash.com/photo-1565992441121-4367c2967103?w=800&h=600&fit=crop" },
        new SkiResort { Name = "Stranda", Latitude = 62.3167, Longitude = 6.9500, Region = "Møre og Romsdal", Elevation = 1035, ImageUrl = "https://images.unsplash.com/photo-1506905925346-21bda4d32df4?w=800&h=600&fit=crop" },
        new SkiResort { Name = "Narvik", Latitude = 68.4333, Longitude = 17.4167, Region = "Nordland", Elevation = 1000, ImageUrl = "https://images.unsplash.com/photo-1513807016779-d51c0c026263?w=800&h=600&fit=crop" }
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
