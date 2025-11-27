const API_BASE = '/api/freeskiing';
let currentFilter = 'all';
let allAreas = [];
let map = null;
let markers = [];

// Initialize app when page loads
document.addEventListener('DOMContentLoaded', () => {
    initMap();
    loadAreas();

    document.getElementById('allAreasBtn').addEventListener('click', () => {
        setFilter('all');
    });

    document.getElementById('intermediateBtn').addEventListener('click', () => {
        setFilter('Intermediate');
    });

    document.getElementById('advancedBtn').addEventListener('click', () => {
        setFilter('Advanced');
    });

    document.getElementById('expertBtn').addEventListener('click', () => {
        setFilter('Expert');
    });
});

function initMap() {
    // Center map on Norway
    map = L.map('map').setView([62.0, 10.0], 5);

    // Add OpenStreetMap tiles
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© OpenStreetMap contributors',
        maxZoom: 19
    }).addTo(map);
}

function updateMapMarkers() {
    // Clear existing markers
    markers.forEach(marker => map.removeLayer(marker));
    markers = [];

    let areasToShow = allAreas;
    if (currentFilter !== 'all') {
        areasToShow = allAreas.filter(area => area.difficulty === currentFilter);
    }

    // Add markers for each area
    areasToShow.forEach(area => {
        const difficultyColor = getDifficultyColor(area.difficulty);

        const marker = L.marker([area.latitude, area.longitude], {
            icon: L.divIcon({
                className: 'custom-marker',
                html: `<div style="background-color: ${difficultyColor}; width: 30px; height: 30px; border-radius: 50%; border: 3px solid white; box-shadow: 0 2px 5px rgba(0,0,0,0.3);"></div>`,
                iconSize: [30, 30],
                iconAnchor: [15, 15]
            })
        }).addTo(map);

        // Add popup with area info
        marker.bindPopup(`
            <div style="min-width: 200px;">
                <h3 style="margin: 0 0 8px 0; color: #333;">${area.name}</h3>
                <p style="margin: 0 0 5px 0; color: #666;"><strong>Region:</strong> ${area.region}</p>
                <p style="margin: 0 0 5px 0; color: #666;"><strong>Difficulty:</strong> <span style="color: ${difficultyColor}; font-weight: bold;">${area.difficulty}</span></p>
                <p style="margin: 0 0 5px 0; color: #666;"><strong>Elevation:</strong> ${area.elevation}m</p>
                <p style="margin: 0; color: #555; font-size: 0.9em;">${area.description}</p>
            </div>
        `);

        markers.push(marker);
    });

    // Fit map to show all markers if there are any
    if (markers.length > 0) {
        const group = L.featureGroup(markers);
        map.fitBounds(group.getBounds().pad(0.1));
    }
}

function setFilter(filter) {
    currentFilter = filter;

    // Update active button
    document.querySelectorAll('.controls button').forEach(btn => {
        btn.classList.remove('active');
    });

    if (filter === 'all') {
        document.getElementById('allAreasBtn').classList.add('active');
    } else if (filter === 'Intermediate') {
        document.getElementById('intermediateBtn').classList.add('active');
    } else if (filter === 'Advanced') {
        document.getElementById('advancedBtn').classList.add('active');
    } else if (filter === 'Expert') {
        document.getElementById('expertBtn').classList.add('active');
    }

    renderAreas();
    updateMapMarkers();
}

async function loadAreas() {
    const loadingEl = document.getElementById('loading');
    const errorEl = document.getElementById('error');
    const areasEl = document.getElementById('areasContainer');

    loadingEl.style.display = 'block';
    errorEl.style.display = 'none';
    areasEl.innerHTML = '';

    try {
        const response = await fetch(`${API_BASE}/areas`);

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        allAreas = await response.json();
        loadingEl.style.display = 'none';

        if (allAreas.length === 0) {
            areasEl.innerHTML = '<div class="loading">No freeskiing areas found.</div>';
            return;
        }

        renderAreas();
        updateMapMarkers();

    } catch (error) {
        loadingEl.style.display = 'none';
        errorEl.style.display = 'block';
        errorEl.textContent = `Error loading data: ${error.message}`;
        console.error('Error:', error);
    }
}

function renderAreas() {
    const container = document.getElementById('areasContainer');
    container.innerHTML = '';

    let filteredAreas = allAreas;

    if (currentFilter !== 'all') {
        filteredAreas = allAreas.filter(area => area.difficulty === currentFilter);
    }

    filteredAreas.forEach(area => {
        const card = createAreaCard(area);
        container.appendChild(card);
    });
}

function createAreaCard(area) {
    const card = document.createElement('div');
    card.className = `area-card difficulty-${area.difficulty.toLowerCase()}`;

    const difficultyColor = getDifficultyColor(area.difficulty);

    card.innerHTML = `
        ${area.imageUrl ? `<div class="area-image" style="background-image: url('${area.imageUrl}');"></div>` : ''}
        <div class="area-header">
            <div class="area-name">${area.name}</div>
            <div class="difficulty-badge" style="background: ${difficultyColor};">${area.difficulty}</div>
        </div>
        <div class="area-info">
            <div class="info-item">
                <span class="info-icon">📍</span>
                <span>${area.region}</span>
            </div>
            <div class="info-item">
                <span class="info-icon">⛰️</span>
                <span>${area.elevation}m elevation</span>
            </div>
            <div class="info-item">
                <span class="info-icon">🎿</span>
                <span>${area.access}</span>
            </div>
            ${area.avalancheGearRequired ? '<div class="info-item warning"><span class="info-icon">⚠️</span><span>Avalanche gear required</span></div>' : ''}
        </div>
        <div class="area-description">
            ${area.description}
        </div>
        <div class="area-details">
            <div class="detail-item">
                <div class="detail-label">Terrain</div>
                <div class="detail-value">${area.terrain}</div>
            </div>
            <div class="detail-item">
                <div class="detail-label">Best Months</div>
                <div class="detail-value">${area.bestMonths}</div>
            </div>
        </div>
    `;

    return card;
}

function getDifficultyColor(difficulty) {
    switch(difficulty) {
        case 'Beginner':
            return '#4CAF50';
        case 'Intermediate':
            return '#2196F3';
        case 'Advanced':
            return '#FF9800';
        case 'Expert':
            return '#f44336';
        default:
            return '#757575';
    }
}
