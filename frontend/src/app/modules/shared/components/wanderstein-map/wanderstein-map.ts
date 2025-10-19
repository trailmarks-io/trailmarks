import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LeafletModule } from '@bluehalo/ngx-leaflet';
import * as L from 'leaflet';
import { WandersteinResponse } from '../../../hiking-stones';

@Component({
  selector: 'app-wanderstein-map',
  standalone: true,
  imports: [CommonModule, LeafletModule],
  templateUrl: './wanderstein-map.html'
})
export class WandersteinMapComponent implements OnInit, OnDestroy {
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

  markers: L.Marker[] = [];
  map: L.Map | null = null;

  ngOnInit(): void {
    // Markers will be initialized when map is ready
  }

  ngOnDestroy(): void {
    if (this.map && this.markers.length > 0) {
      this.markers.forEach(marker => {
        if (this.map) {
          this.map.removeLayer(marker);
        }
      });
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

    this.addMarkers();
    
    // Fit map bounds to show all markers
    if (this.markers.length > 0) {
      const group = L.featureGroup(this.markers);
      map.fitBounds(group.getBounds().pad(0.1));
    }
  }

  private addMarkers(): void {
    if (!this.map) return;

    // Clear existing markers
    this.markers.forEach(marker => {
      if (this.map) {
        this.map.removeLayer(marker);
      }
    });
    this.markers = [];

    // Add new markers
    this.wandersteine.forEach(wanderstein => {
      if (wanderstein.latitude && wanderstein.longitude && this.map) {
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
        marker.addTo(this.map);
        this.markers.push(marker);
      }
    });
  }

  private escapeHtml(text: string): string {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
  }
}
