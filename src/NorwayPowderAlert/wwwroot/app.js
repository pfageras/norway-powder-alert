const API_BASE = '/api/powder';
let showPowderOnly = false;
let resortMap = null;
let resortMarkers = [];

// Initialize app when page loads
document.addEventListener('DOMContentLoaded', () => {
    initResortMap();
    loadAlerts();

    document.getElementById('refreshBtn').addEventListener('click', () => {
        loadAlerts();
    });

    document.getElementById('powderOnlyBtn').addEventListener('click', () => {
        showPowderOnly = !showPowderOnly;
        const btn = document.getElementById('powderOnlyBtn');
        btn.textContent = showPowderOnly ? 'Show All Resorts' : 'Show Powder Days Only';
        loadAlerts();
    });
});

function initResortMap() {
    // Center map on Norway
    resortMap = L.map('resortMap').setView([61.5, 9.0], 5);

    // Add OpenStreetMap tiles
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© OpenStreetMap contributors',
        maxZoom: 19
    }).addTo(resortMap);
}

function updateResortMap(alerts) {
    // Clear existing markers
    resortMarkers.forEach(marker => resortMap.removeLayer(marker));
    resortMarkers = [];

    // Add markers for each resort
    alerts.forEach(alert => {
        const resort = alert.resort;
        const isPowderDay = alert.isPowderDay;
        const markerColor = isPowderDay ? '#4CAF50' : '#2196F3';

        const marker = L.marker([resort.latitude, resort.longitude], {
            icon: L.divIcon({
                className: 'custom-marker',
                html: `<div style="background-color: ${markerColor}; width: ${isPowderDay ? 35 : 25}px; height: ${isPowderDay ? 35 : 25}px; border-radius: 50%; border: 3px solid white; box-shadow: 0 2px 5px rgba(0,0,0,0.3); ${isPowderDay ? 'animation: pulse 2s infinite;' : ''}"></div>`,
                iconSize: [isPowderDay ? 35 : 25, isPowderDay ? 35 : 25],
                iconAnchor: [isPowderDay ? 17.5 : 12.5, isPowderDay ? 17.5 : 12.5]
            })
        }).addTo(resortMap);

        // Add popup with resort info
        marker.bindPopup(`
            <div style="min-width: 250px;">
                <h3 style="margin: 0 0 8px 0; color: #333;">${resort.name}</h3>
                ${isPowderDay ? '<p style="margin: 0 0 8px 0; color: #4CAF50; font-weight: bold;">🎿 POWDER DAY!</p>' : ''}
                <p style="margin: 0 0 5px 0;"><strong>24h Snow:</strong> ${alert.snowfallCm24h} cm</p>
                <p style="margin: 0 0 5px 0;"><strong>48h Snow:</strong> ${alert.snowfallCm48h} cm</p>
                <p style="margin: 0 0 5px 0;"><strong>7-day Total:</strong> ${alert.snowfallCm7Day} cm</p>
                <p style="margin: 0 0 5px 0;"><strong>Snow Depth:</strong> ${alert.currentSnowDepthCm} cm</p>
                <p style="margin: 0 0 5px 0;"><strong>Elevation:</strong> ${resort.elevation}m</p>
                ${resort.webcamUrl ? `<p style="margin: 5px 0 0 0;"><a href="${resort.webcamUrl}" target="_blank" style="color: #2196F3;">View Webcam</a></p>` : ''}
            </div>
        `);

        resortMarkers.push(marker);
    });

    // Fit map to show all markers if there are any
    if (resortMarkers.length > 0) {
        const group = L.featureGroup(resortMarkers);
        resortMap.fitBounds(group.getBounds().pad(0.1));
    }
}

async function loadAlerts() {
    const loadingEl = document.getElementById('loading');
    const errorEl = document.getElementById('error');
    const alertsEl = document.getElementById('powderAlerts');

    loadingEl.style.display = 'block';
    errorEl.style.display = 'none';
    alertsEl.innerHTML = '';

    try {
        const endpoint = showPowderOnly ? `${API_BASE}/powder-days` : `${API_BASE}/alerts`;
        const response = await fetch(endpoint);

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const alerts = await response.json();
        loadingEl.style.display = 'none';

        if (alerts.length === 0) {
            alertsEl.innerHTML = '<div class="loading">No powder alerts found. Check back later!</div>';
            return;
        }

        renderAlerts(alerts);
        updateResortMap(alerts);
        updateLastUpdateTime();

    } catch (error) {
        loadingEl.style.display = 'none';
        errorEl.style.display = 'block';
        errorEl.textContent = `Error loading data: ${error.message}`;
        console.error('Error:', error);
    }
}

function renderAlerts(alerts) {
    const container = document.getElementById('powderAlerts');
    container.innerHTML = '';

    alerts.forEach(alert => {
        const card = createAlertCard(alert);
        container.appendChild(card);
    });

    // Add click handlers for collapsible sections
    document.querySelectorAll('.section-title').forEach(title => {
        title.addEventListener('click', function() {
            this.classList.toggle('collapsed');
            const content = this.nextElementSibling;
            if (content && content.classList.contains('daily-breakdown-content')) {
                content.classList.toggle('hidden');
            }
        });
    });
}

function createAlertCard(alert) {
    const card = document.createElement('div');
    card.className = `alert-card ${alert.isPowderDay ? 'powder-day' : ''}`;

    const resort = alert.resort;
    const snow24h = alert.snowfallCm24h.toFixed(1);
    const snow48h = alert.snowfallCm48h.toFixed(1);
    const snow3Day = alert.snowfallCm3Day.toFixed(1);
    const snow7Day = alert.snowfallCm7Day.toFixed(1);
    const snow10Day = alert.snowfallCm10Day.toFixed(1);

    card.innerHTML = `
        ${resort.imageUrl ? `<div class="resort-image" style="background-image: url('${resort.imageUrl}');"></div>` : ''}
        <div class="resort-header">
            <div class="resort-name">${resort.name}</div>
            ${alert.isPowderDay ? '<div class="powder-badge">POWDER DAY!</div>' : ''}
        </div>
        <div class="resort-info">
            ${resort.region} • ${resort.elevation}m elevation
            ${resort.webcamUrl ? `<a href="${resort.webcamUrl}" target="_blank" class="webcam-link">📹 Webcam</a>` : ''}
        </div>
        <div class="snow-depth-section">
            <div class="depth-item">
                <div class="depth-label">Snow Depth</div>
                <div class="depth-value">${alert.currentSnowDepthCm} cm</div>
            </div>
            <div class="depth-item">
                <div class="depth-label">Season Total</div>
                <div class="depth-value">${alert.seasonalSnowfallCm} cm</div>
            </div>
        </div>
        <div class="snow-stats">
            <div class="stat stat-small">
                <div class="stat-value">${snow24h}</div>
                <div class="stat-label">24h</div>
            </div>
            <div class="stat stat-small">
                <div class="stat-value">${snow48h}</div>
                <div class="stat-label">48h</div>
            </div>
            <div class="stat stat-small">
                <div class="stat-value">${snow3Day}</div>
                <div class="stat-label">3 days</div>
            </div>
        </div>
        <div class="long-term-stats">
            <div class="stat-row">
                <span class="stat-row-label">7-day total:</span>
                <span class="stat-row-value">${snow7Day} cm</span>
            </div>
            <div class="stat-row">
                <span class="stat-row-label">10-day total:</span>
                <span class="stat-row-value">${snow10Day} cm</span>
            </div>
        </div>
        <div class="daily-breakdown">
            <div class="section-title collapsed">10-Day Forecast</div>
            ${renderDailyBreakdown(alert.dailyBreakdown)}
        </div>
        <div class="forecast-preview">
            <div class="forecast-title">Next 24 hours:</div>
            <div class="forecast-hours">
                ${renderForecastPreview(alert.forecast)}
            </div>
        </div>
    `;

    return card;
}

function renderDailyBreakdown(dailyBreakdown) {
    if (!dailyBreakdown || dailyBreakdown.length === 0) {
        return '<div class="no-data">No daily forecast available</div>';
    }

    const content = dailyBreakdown.map(day => {
        const date = new Date(day.date);
        const dayName = date.toLocaleDateString('en-US', { weekday: 'short' });
        const dateStr = date.toLocaleDateString('en-US', { month: 'short', day: 'numeric' });
        const snowfall = day.snowfallCm.toFixed(1);
        const tempRange = `${day.minTemp.toFixed(0)}°/${day.maxTemp.toFixed(0)}°`;

        // Determine if it's a good snow day
        const isGoodDay = day.snowfallCm >= 10;
        const dayClass = isGoodDay ? 'daily-item good-snow' : 'daily-item';

        return `
            <div class="${dayClass}">
                <div class="daily-date">
                    <div class="day-name">${dayName}</div>
                    <div class="date-str">${dateStr}</div>
                </div>
                <div class="daily-snow">${snowfall} cm</div>
                <div class="daily-temp">${tempRange}</div>
                <div class="daily-conditions">${day.conditions}</div>
            </div>
        `;
    }).join('');

    return `<div class="daily-breakdown-content hidden">${content}</div>`;
}

function renderForecastPreview(forecast) {
    if (!forecast || forecast.length === 0) {
        return '<div>No forecast data available</div>';
    }

    // Show next 8 hours
    const hours = forecast.slice(0, 8);

    return hours.map(hour => {
        const time = new Date(hour.time);
        const timeStr = time.toLocaleTimeString('en-US', { hour: '2-digit', minute: '2-digit' });
        const snowMm = hour.snowfallMm.toFixed(1);
        const temp = hour.temperature.toFixed(1);

        return `
            <div class="forecast-hour">
                <div class="forecast-time">${timeStr}</div>
                <div class="forecast-snow">${snowMm}mm</div>
                <div class="forecast-temp">${temp}°C</div>
            </div>
        `;
    }).join('');
}

function updateLastUpdateTime() {
    const now = new Date();
    const timeStr = now.toLocaleTimeString('en-US', {
        hour: '2-digit',
        minute: '2-digit',
        second: '2-digit'
    });
    document.getElementById('lastUpdate').textContent = `Last update: ${timeStr}`;
}
