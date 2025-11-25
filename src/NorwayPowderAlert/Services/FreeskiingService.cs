using NorwayPowderAlert.Models;

namespace NorwayPowderAlert.Services;

public class FreeskiingService
{
    private readonly List<FreeskiingArea> _areas = new()
    {
        new FreeskiingArea
        {
            Name = "Stetind (Hemsedal)",
            Latitude = 60.8630,
            Longitude = 8.5390,
            Region = "Buskerud",
            Elevation = 1700,
            Description = "Iconic freeskiing terrain with steep couloirs and powder bowls. One of Norway's most famous off-piste areas.",
            Difficulty = "Expert",
            Terrain = "Couloirs, Steep faces, Powder bowls",
            Access = "Lift-accessed + short hike",
            ImageUrl = "https://images.unsplash.com/photo-1551524164-687a55dd1126?w=800&h=600&fit=crop",
            AvalancheGearRequired = true,
            BestMonths = "February - April"
        },
        new FreeskiingArea
        {
            Name = "Narvikfjellet Backcountry",
            Latitude = 68.4333,
            Longitude = 17.4167,
            Region = "Nordland",
            Elevation = 1000,
            Description = "Arctic freeskiing with incredible fjord views. Long descents from mountain to sea level.",
            Difficulty = "Advanced",
            Terrain = "Steep bowls, Ridge lines, Powder fields",
            Access = "Lift-accessed",
            ImageUrl = "https://images.unsplash.com/photo-1513807016779-d51c0c026263?w=800&h=600&fit=crop",
            AvalancheGearRequired = true,
            BestMonths = "March - May"
        },
        new FreeskiingArea
        {
            Name = "Stranda Backcountry",
            Latitude = 62.3167,
            Longitude = 6.9500,
            Region = "Møre og Romsdal",
            Elevation = 1200,
            Description = "World-class freeriding with ocean views. Host of the Freeride World Tour.",
            Difficulty = "Expert",
            Terrain = "Steep couloirs, Open faces, Natural features",
            Access = "Lift-accessed",
            ImageUrl = "https://images.unsplash.com/photo-1506905925346-21bda4d32df4?w=800&h=600&fit=crop",
            AvalancheGearRequired = true,
            BestMonths = "February - May"
        },
        new FreeskiingArea
        {
            Name = "Myrkdalen Freeride",
            Latitude = 60.8833,
            Longitude = 6.8667,
            Region = "Vestland",
            Elevation = 1400,
            Description = "Deep powder and tree skiing in Norway's snowiest valley. Consistent snowfall.",
            Difficulty = "Intermediate",
            Terrain = "Tree runs, Powder bowls, Gladed terrain",
            Access = "Lift-accessed",
            ImageUrl = "https://images.unsplash.com/photo-1519904981063-b0cf448d479e?w=800&h=600&fit=crop",
            AvalancheGearRequired = true,
            BestMonths = "December - April"
        },
        new FreeskiingArea
        {
            Name = "Lyngen Alps",
            Latitude = 69.5800,
            Longitude = 20.2200,
            Region = "Troms",
            Elevation = 1500,
            Description = "Premier ski touring destination with dramatic alpine terrain and midnight sun skiing in spring.",
            Difficulty = "Expert",
            Terrain = "Couloirs, Alpine faces, Glaciers",
            Access = "Ski touring",
            ImageUrl = "https://images.unsplash.com/photo-1551524164-687a55dd1126?w=800&h=600&fit=crop&brightness=0.9",
            AvalancheGearRequired = true,
            BestMonths = "March - May"
        },
        new FreeskiingArea
        {
            Name = "Romsdalen Valley",
            Latitude = 62.5600,
            Longitude = 7.8400,
            Region = "Møre og Romsdal",
            Elevation = 1800,
            Description = "Big mountain skiing with massive vertical drops. Home to legendary BASE jumping cliffs.",
            Difficulty = "Expert",
            Terrain = "Big mountain, Steep faces, Exposed ridges",
            Access = "Ski touring + approach",
            ImageUrl = "https://images.unsplash.com/photo-1551524164-687a55dd1126?w=800&h=600&fit=crop&sat=-20",
            AvalancheGearRequired = true,
            BestMonths = "April - June"
        },
        new FreeskiingArea
        {
            Name = "Oppdal Backcountry",
            Latitude = 62.5969,
            Longitude = 9.6844,
            Region = "Trøndelag",
            Elevation = 1400,
            Description = "Vast terrain with gentle bowls and steeper lines. Great for all levels of freeriding.",
            Difficulty = "Intermediate",
            Terrain = "Powder bowls, Tree skiing, Alpine terrain",
            Access = "Lift-accessed",
            ImageUrl = "https://images.unsplash.com/photo-1548704888-e2e6cc6c81ed?w=800&h=600&fit=crop",
            AvalancheGearRequired = true,
            BestMonths = "January - April"
        },
        new FreeskiingArea
        {
            Name = "Voss Freeride",
            Latitude = 60.6272,
            Longitude = 6.4258,
            Region = "Vestland",
            Elevation = 1000,
            Description = "Playful terrain with natural features and good tree skiing. Home to Voss Freeride competition.",
            Difficulty = "Advanced",
            Terrain = "Natural jumps, Tree runs, Steep sections",
            Access = "Lift-accessed",
            ImageUrl = "https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?w=800&h=600&fit=crop",
            AvalancheGearRequired = true,
            BestMonths = "January - April"
        },
        new FreeskiingArea
        {
            Name = "Jotunheimen National Park",
            Latitude = 61.5700,
            Longitude = 8.3500,
            Region = "Oppland",
            Elevation = 2000,
            Description = "Norway's premier ski touring destination with high alpine terrain and glacier skiing.",
            Difficulty = "Expert",
            Terrain = "Glaciers, Alpine bowls, High mountain",
            Access = "Ski touring",
            ImageUrl = "https://images.unsplash.com/photo-1605540436563-5bca919ae766?w=800&h=600&fit=crop&brightness=1.1",
            AvalancheGearRequired = true,
            BestMonths = "April - June"
        },
        new FreeskiingArea
        {
            Name = "Kvitfjell Off-Piste",
            Latitude = 61.4558,
            Longitude = 10.1458,
            Region = "Innlandet",
            Elevation = 1030,
            Description = "Olympic downhill venue with excellent off-piste terrain through forests and open bowls.",
            Difficulty = "Intermediate",
            Terrain = "Tree runs, Open bowls, Groomers access",
            Access = "Lift-accessed",
            ImageUrl = "https://images.unsplash.com/photo-1551524164-687a55dd1126?w=800&h=600&fit=crop&sat=-10",
            AvalancheGearRequired = true,
            BestMonths = "January - March"
        }
    };

    public List<FreeskiingArea> GetAllAreas()
    {
        return _areas;
    }

    public FreeskiingArea? GetAreaByName(string name)
    {
        return _areas.FirstOrDefault(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public List<FreeskiingArea> GetAreasByDifficulty(string difficulty)
    {
        return _areas.Where(a => a.Difficulty.Equals(difficulty, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<FreeskiingArea> GetAreasByRegion(string region)
    {
        return _areas.Where(a => a.Region.Equals(region, StringComparison.OrdinalIgnoreCase)).ToList();
    }
}
