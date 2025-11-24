# Norway Powder Alert

A real-time powder conditions dashboard for Norwegian ski resorts. Get instant alerts when fresh snow (20cm+ in 24h) is forecast at major Norwegian ski areas.

## Features

- **Real-time Weather Data**: Fetches live weather data from the Norwegian Meteorological Institute (met.no)
- **14 Major Resorts**: Monitors Hemsedal, Trysil, Geilo, Hafjell, Kvitfjell, Norefjell, Myrkdalen, Voss, Oppdal, Hovden, Skeikampen, Kongsberg, Stranda, and Narvik
- **Powder Day Detection**: Automatically identifies when resorts receive 20cm+ of fresh snow in 24 hours
- **Long-Term Forecasts**: View snowfall predictions for 24h, 48h, 3-day, 7-day, and 10-day periods
- **10-Day Daily Breakdown**: Detailed day-by-day forecast showing snowfall, temperature range, and conditions
- **Visual Indicators**: Days with 10cm+ snowfall are highlighted for easy planning
- **Simple Dashboard**: Clean, responsive web interface to view conditions at a glance
- **Docker Ready**: Fully containerized for easy deployment

## Tech Stack

- **Backend**: ASP.NET Core 8.0 Web API (C#)
- **Frontend**: Vanilla HTML/CSS/JavaScript
- **Weather API**: met.no LocationForecast API
- **Deployment**: Docker & Docker Compose

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (for local development)
- [Docker](https://www.docker.com/get-started) (for containerized deployment)

### Option 1: Run with Docker (Recommended)

1. Clone the repository:
   ```bash
   cd norway-powder-alert
   ```

2. Build and run with Docker Compose:
   ```bash
   docker-compose up --build
   ```

3. Open your browser and navigate to:
   ```
   http://localhost:8080
   ```

### Option 2: Run Locally with .NET

1. Navigate to the project directory:
   ```bash
   cd norway-powder-alert/src/NorwayPowderAlert
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Run the application:
   ```bash
   dotnet run
   ```

4. Open your browser and navigate to:
   ```
   http://localhost:5000
   ```

## API Endpoints

The application exposes the following REST API endpoints:

- `GET /api/powder/alerts` - Get all resort alerts (sorted by snowfall)
- `GET /api/powder/powder-days` - Get only resorts with powder conditions (20cm+)
- `GET /api/powder/resort/{name}` - Get alert for a specific resort
- `GET /api/powder/resorts` - Get list of all monitored resorts

### Example API Response

```json
{
  "resort": {
    "name": "Hemsedal",
    "latitude": 60.8630,
    "longitude": 8.5390,
    "region": "Buskerud",
    "elevation": 1450
  },
  "snowfallCm24h": 25.5,
  "snowfallCm48h": 42.3,
  "snowfallCm3Day": 58.2,
  "snowfallCm7Day": 95.0,
  "snowfallCm10Day": 115.5,
  "forecastTime": "2025-01-15T10:30:00Z",
  "isPowderDay": true,
  "forecast": [...],
  "dailyBreakdown": [
    {
      "date": "2025-01-15T00:00:00Z",
      "snowfallCm": 25.5,
      "minTemp": -5.2,
      "maxTemp": -1.0,
      "conditions": "Heavy Snow"
    },
    ...
  ]
}
```

## Dashboard Features

- **All Resorts View**: See conditions at all 14 monitored resorts
- **Powder Days Only**: Filter to show only resorts with 20cm+ fresh snow
- **Long-Term Forecasts**: View 24h, 48h, 3-day, 7-day, and 10-day snowfall totals
- **10-Day Breakdown**: Day-by-day forecast with snowfall amounts, temps, and conditions
- **Smart Highlighting**: Days with 10cm+ snow are highlighted in green for easy trip planning
- **Auto-Refresh**: Manually refresh data to get the latest conditions
- **Responsive Design**: Works on desktop, tablet, and mobile devices
- **Visual Indicators**: Powder days are highlighted with green borders and badges

## Customization

### Change Powder Threshold

Edit `src/NorwayPowderAlert/Services/PowderAlertService.cs`:

```csharp
private readonly double _powderThresholdCm = 20.0; // Change this value
```

### Add More Resorts

Edit `src/NorwayPowderAlert/Services/ResortService.cs` and add to the `_resorts` list:

```csharp
new SkiResort {
    Name = "Your Resort",
    Latitude = 60.0000,
    Longitude = 8.0000,
    Region = "Region Name",
    Elevation = 1200
}
```

### Modify User-Agent for met.no API

The Norwegian Meteorological Institute requires a proper User-Agent header. Edit `src/NorwayPowderAlert/Services/WeatherService.cs`:

```csharp
_httpClient.DefaultRequestHeaders.Add("User-Agent",
    "YourAppName/1.0 github.com/yourusername/yourrepo");
```

## Docker Deployment

### Build the image:
```bash
docker build -t norway-powder-alert .
```

### Run the container:
```bash
docker run -p 8080:8080 norway-powder-alert
```

### Using Docker Compose:
```bash
docker-compose up -d
```

### Stop the container:
```bash
docker-compose down
```

## Development

### Project Structure

```
norway-powder-alert/
├── src/
│   └── NorwayPowderAlert/
│       ├── Controllers/          # API controllers
│       │   └── PowderController.cs
│       ├── Models/               # Data models
│       │   ├── SkiResort.cs
│       │   ├── WeatherForecast.cs
│       │   └── PowderAlert.cs
│       ├── Services/             # Business logic
│       │   ├── ResortService.cs
│       │   ├── WeatherService.cs
│       │   └── PowderAlertService.cs
│       ├── wwwroot/              # Frontend files
│       │   ├── index.html
│       │   ├── styles.css
│       │   └── app.js
│       ├── Program.cs            # App entry point
│       └── NorwayPowderAlert.csproj
├── Dockerfile
├── docker-compose.yml
└── README.md
```

## License

MIT License - feel free to use this for your own powder tracking needs!

## Credits

- Weather data provided by the [Norwegian Meteorological Institute](https://api.met.no/)
- Built with ASP.NET Core and love for powder skiing

---

**Happy powder hunting!** May the snow gods bless you with deep pow!
