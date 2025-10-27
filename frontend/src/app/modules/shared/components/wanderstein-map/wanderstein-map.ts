import { Component, Input, Output, EventEmitter, OnInit, OnDestroy, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LeafletModule } from '@bluehalo/ngx-leaflet';
import * as L from 'leaflet';
import 'leaflet.markercluster';
import { WandersteinResponse } from '../../../hiking-stones';

// Extend L type to include markerClusterGroup
declare global {
  interface Window {
    L: typeof L & {
      markerClusterGroup: (options?: any) => any;
    };
  }
}

@Component({
  selector: 'app-wanderstein-map',
  standalone: true,
  imports: [CommonModule, LeafletModule],
  templateUrl: './wanderstein-map.html'
})
export class WandersteinMapComponent implements OnInit, OnDestroy, OnChanges {
  @Input() wandersteine: WandersteinResponse[] = [];
  @Input() enableDynamicLoading: boolean = false;
  @Input() vignetteCenter?: { lat: number, lng: number };
  @Input() vignetteRadiusKm: number = 50; // Default 50km radius
  @Output() markerClick = new EventEmitter<string>();
  @Output() locationChange = new EventEmitter<{latitude: number, longitude: number, radiusKm: number}>();

  options: L.MapOptions = {
    layers: [
      L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 18,
        attribution: '© OpenStreetMap contributors'
      })
    ],
    zoom: 2,
    center: L.latLng(20, 0)
  };

  markerClusterGroup: any = null;
  map: L.Map | null = null;
  vignetteCircle: L.Circle | null = null;
  vignetteMask: L.Polygon | null = null;
  private clusteringEndListener: (() => void) | null = null;
  private userLocation: L.LatLng | null = null;

  ngOnInit(): void {
    // Marker cluster will be initialized when map is ready
    if (this.enableDynamicLoading) {
      this.getUserLocation();
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    // When wandersteine input changes, refresh markers if map is ready
    if (changes['wandersteine'] && !changes['wandersteine'].firstChange && this.map && this.markerClusterGroup) {
      this.refreshMarkers();
    }
    // When vignette settings change, redraw vignette
    if ((changes['vignetteCenter'] || changes['vignetteRadiusKm']) && this.map) {
      this.drawVignette();
    }
  }

  ngOnDestroy(): void {
    // Clean up event listeners
    if (this.clusteringEndListener && this.markerClusterGroup) {
      this.markerClusterGroup.off('animationend', this.clusteringEndListener);
      this.clusteringEndListener = null;
    }
    
    if (this.markerClusterGroup && this.map) {
      this.map.removeLayer(this.markerClusterGroup);
      this.markerClusterGroup = null;
    }

    // Clean up vignette layers
    this.removeVignetteLayers();
  }

  onMapReady(map: L.Map): void {
    this.map = map;
    
    // Fix marker icon issue with Leaflet in Angular
    const iconDefault = L.icon({
      iconUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-icon.png',
      shadowUrl: 'https://unpkg.com/leaflet@1.9.4/dist/images/marker-shadow.png',
      iconSize: [25, 41],
      iconAnchor: [12, 41],
      popupAnchor: [1, -34],
      shadowSize: [41, 41]
    });
    L.Marker.prototype.options.icon = iconDefault;

    // Create marker cluster group - use global L object
    const LGlobal = (window as any).L || L;
    this.markerClusterGroup = LGlobal.markerClusterGroup({
      maxClusterRadius: 80,
      spiderfyOnMaxZoom: true,
      showCoverageOnHover: true,
      zoomToBoundsOnClick: true,
      disableClusteringAtZoom: 15
    });

    // Set up event listener for clustering completion
    this.clusteringEndListener = () => {
      this.updateMapBounds();
    };
    this.markerClusterGroup.on('animationend', this.clusteringEndListener);

    this.addMarkers();
    
    if (this.markerClusterGroup) {
      map.addLayer(this.markerClusterGroup);
      // Initial bounds update after markers are added
      this.updateMapBounds();
    }

    // Draw initial vignette if center is provided
    this.drawVignette();

    // If dynamic loading is enabled, set up event handlers
    if (this.enableDynamicLoading) {
      // Set initial center based on user location or default to Bochum
      if (this.userLocation) {
        map.setView(this.userLocation, 10);
      } else {
        // Default to Bochum, Germany
        map.setView(L.latLng(51.4818, 7.2162), 9);
      }
      
      // Emit initial location
      this.emitCurrentLocation();

      // Listen for map move/zoom events
      map.on('moveend', () => this.emitCurrentLocation());
      map.on('zoomend', () => this.emitCurrentLocation());
    }
  }

  private getUserLocation(): void {
    // Guard for SSR - check if window and navigator are available
    if (typeof window === 'undefined' || !navigator || !navigator.geolocation) {
      return;
    }

    const geolocationOptions: PositionOptions = {
      timeout: 5000,
      maximumAge: 0,
      enableHighAccuracy: false
    };

    navigator.geolocation.getCurrentPosition(
      (position) => {
        this.userLocation = L.latLng(position.coords.latitude, position.coords.longitude);
        if (this.map) {
          this.map.setView(this.userLocation, 10);
          this.emitCurrentLocation();
        }
      },
      (error) => {
        // Geolocation failed, use default location (Bochum)
        console.log('Geolocation failed, using default location (Bochum)', error);
        this.userLocation = null;
        
        // Set map to Bochum as fallback
        const bochumLocation = L.latLng(51.4818, 7.2162);
        if (this.map) {
          this.map.setView(bochumLocation, 9);
          this.emitCurrentLocation();
        }
      },
      geolocationOptions
    );
  }

  private emitCurrentLocation(): void {
    if (!this.map) return;

    const center = this.map.getCenter();
    const bounds = this.map.getBounds();
    
    // Calculate radius based on map bounds (use the larger of the two distances)
    const northEast = bounds.getNorthEast();
    const southWest = bounds.getSouthWest();
    const latDiff = northEast.lat - southWest.lat;
    const lngDiff = northEast.lng - southWest.lng;
    
    // Approximate km per degree (varies by latitude, but good enough for this purpose)
    const kmPerDegreeLat = 111;
    const kmPerDegreeLng = 111 * Math.cos(center.lat * Math.PI / 180);
    
    const radiusLat = (latDiff * kmPerDegreeLat) / 2;
    const radiusLng = (lngDiff * kmPerDegreeLng) / 2;
    const radiusKm = Math.max(radiusLat, radiusLng);

    this.locationChange.emit({
      latitude: center.lat,
      longitude: center.lng,
      radiusKm: Math.ceil(radiusKm)
    });
  }

  private addMarkers(): void {
    if (!this.markerClusterGroup) return;

    // Clear existing markers to make this idempotent
    this.markerClusterGroup.clearLayers();

    this.wandersteine.forEach(wanderstein => {
      if (wanderstein.latitude && wanderstein.longitude) {
        const marker = L.marker([wanderstein.latitude, wanderstein.longitude]);
        
        // Sanitize data by escaping HTML entities
        const escapedName = this.escapeHtml(wanderstein.name);
        const escapedId = this.escapeHtml(wanderstein.unique_Id);
        const escapedUrl = this.escapeHtml(wanderstein.preview_Url || '');
        
        const popupContent = `
          <div class="min-w-[200px]">
            <h3 class="m-0 mb-2 font-semibold">${escapedName}</h3>
            <p class="my-1 text-gray-500 text-sm"><strong>ID:</strong> ${escapedId}</p>
            ${wanderstein.preview_Url ? `<img src="${escapedUrl}" alt="${escapedName}" class="w-full mt-2 rounded">` : ''}
            <button id="view-details-${escapedId}" class="mt-2 px-3 py-1.5 bg-blue-500 text-white border-none rounded cursor-pointer w-full hover:bg-blue-600 transition-colors">
              View Details
            </button>
          </div>
        `;
        
        marker.bindPopup(popupContent);
        
        // Add click event to the button after popup is opened
        marker.on('popupopen', () => {
          const button = document.getElementById(`view-details-${escapedId}`);
          if (button) {
            button.addEventListener('click', () => {
              this.markerClick.emit(wanderstein.unique_Id);
            });
          }
        });
        
        this.markerClusterGroup.addLayer(marker);
      }
    });
  }

  private refreshMarkers(): void {
    // Refresh markers when input changes
    this.addMarkers();
    this.updateMapBounds();
  }

  private updateMapBounds(): void {
    // Only update bounds if we have a map, cluster group, and markers
    if (!this.map || !this.markerClusterGroup) return;
    
    const markersWithCoords = this.wandersteine.filter(w => w.latitude && w.longitude);
    if (markersWithCoords.length === 0) return;
    
    const bounds = this.markerClusterGroup.getBounds();
    if (bounds && bounds.isValid()) {
      this.map.fitBounds(bounds.pad(0.1));
    }
  }

  private escapeHtml(text: string): string {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
  }

  private removeVignetteLayers(): void {
    if (this.vignetteCircle && this.map) {
      this.map.removeLayer(this.vignetteCircle);
      this.vignetteCircle = null;
    }
    if (this.vignetteMask && this.map) {
      this.map.removeLayer(this.vignetteMask);
      this.vignetteMask = null;
    }
  }

  private drawVignette(): void {
    // Remove existing vignette layers first
    this.removeVignetteLayers();

    // Only draw if we have a map and vignette center
    if (!this.map || !this.vignetteCenter) {
      return;
    }

    const centerLatLng = L.latLng(this.vignetteCenter.lat, this.vignetteCenter.lng);
    const radiusMeters = this.vignetteRadiusKm * 1000;

    // Draw vignette circle (outline showing the focus area)
    this.vignetteCircle = L.circle(centerLatLng, {
      radius: radiusMeters,
      color: '#3b82f6',
      fillColor: 'transparent',
      fillOpacity: 0,
      weight: 2,
      interactive: false
    });
    this.vignetteCircle.addTo(this.map);

    // Draw vignette mask (grey out area outside the circle)
    const bounds = this.map.getBounds();
    const sw = bounds.getSouthWest();
    const ne = bounds.getNorthEast();
    
    // Create outer rectangle with padding
    const outerRing = [
      L.latLng(sw.lat - 10, sw.lng - 10),
      L.latLng(sw.lat - 10, ne.lng + 10),
      L.latLng(ne.lat + 10, ne.lng + 10),
      L.latLng(ne.lat + 10, sw.lng - 10)
    ];

    // Create circle approximation as inner ring (hole in the mask)
    const steps = 64;
    const circleCoords: L.LatLng[] = [];
    for (let i = 0; i < steps; i++) {
      const angle = (i / steps) * 2 * Math.PI;
      const dx = Math.cos(angle);
      const dy = Math.sin(angle);
      
      // Convert meters to degrees (approximation)
      const latOffset = (dy * radiusMeters) / 111320;
      const lngOffset = (dx * radiusMeters) / (40075000 * Math.cos(this.vignetteCenter.lat * Math.PI / 180) / 360);
      
      circleCoords.push(L.latLng(this.vignetteCenter.lat + latOffset, this.vignetteCenter.lng + lngOffset));
    }

    // Create polygon with hole (outer ring and inner circle)
    this.vignetteMask = L.polygon([outerRing, circleCoords], {
      color: 'transparent',
      fillColor: '#000',
      fillOpacity: 0.3,
      weight: 0,
      interactive: false
    });
    this.vignetteMask.addTo(this.map);

    // Bring vignette circle to front so it's visible above the mask
    if (this.vignetteCircle) {
      this.vignetteCircle.bringToFront();
    }
  }
}
