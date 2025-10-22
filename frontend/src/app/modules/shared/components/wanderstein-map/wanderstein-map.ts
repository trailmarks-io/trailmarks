import { Component, Input, OnInit, OnDestroy, OnChanges, SimpleChanges } from '@angular/core';
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
  private clusteringEndListener: (() => void) | null = null;

  ngOnInit(): void {
    // Marker cluster will be initialized when map is ready
  }

  ngOnChanges(changes: SimpleChanges): void {
    // When wandersteine input changes, refresh markers if map is ready
    if (changes['wandersteine'] && !changes['wandersteine'].firstChange && this.map && this.markerClusterGroup) {
      this.refreshMarkers();
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
          <div style="min-width: 200px;">
            <h3 style="margin: 0 0 8px 0; font-weight: 600;">${escapedName}</h3>
            <p style="margin: 4px 0; color: #6b7280; font-size: 14px;"><strong>ID:</strong> ${escapedId}</p>
            ${wanderstein.preview_Url ? `<img src="${escapedUrl}" alt="${escapedName}" style="width: 100%; margin-top: 8px; border-radius: 4px;">` : ''}
          </div>
        `;
        
        marker.bindPopup(popupContent);
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
}
