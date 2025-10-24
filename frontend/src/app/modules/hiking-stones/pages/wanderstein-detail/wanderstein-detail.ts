import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { WandersteinService, WandersteinDetailResponse } from '../../services/wanderstein';
import { LanguageService, TranslatePipe } from '../../../core';
import { LeafletModule } from '@bluehalo/ngx-leaflet';
import * as L from 'leaflet';

@Component({
  selector: 'app-wanderstein-detail',
  imports: [CommonModule, TranslatePipe, LeafletModule],
  templateUrl: './wanderstein-detail.html',
  standalone: true
})
export class WandersteinDetailPage implements OnInit {
  wanderstein: WandersteinDetailResponse | null = null;
  loading = true;
  error: string | null = null;
  
  mapOptions: L.MapOptions = {
    layers: [
      L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 18,
        attribution: '© OpenStreetMap contributors'
      })
    ],
    zoom: 13,
    center: L.latLng(0, 0)
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private wandersteinService: WandersteinService,
    private languageService: LanguageService
  ) {}

  ngOnInit(): void {
    const uniqueId = this.route.snapshot.paramMap.get('uniqueId');
    if (uniqueId) {
      this.loadWandersteinDetails(uniqueId);
    } else {
      this.error = this.languageService.translate('wanderstein.detail.error.noId');
      this.loading = false;
    }
  }

  loadWandersteinDetails(uniqueId: string): void {
    this.loading = true;
    this.error = null;
    
    this.wandersteinService.getWandersteinByUniqueId(uniqueId).subscribe({
      next: (data) => {
        this.wanderstein = data;
        this.loading = false;
        
        // Update map center if coordinates are available
        if (data.latitude && data.longitude) {
          this.mapOptions = {
            ...this.mapOptions,
            center: L.latLng(data.latitude, data.longitude)
          };
        }
      },
      error: (err) => {
        const errorMsg = this.languageService.translate('wanderstein.detail.error');
        this.error = `${errorMsg}: ${err.message}`;
        this.loading = false;
      }
    });
  }

  onMapReady(map: L.Map): void {
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

    // Add marker if coordinates are available
    if (this.wanderstein?.latitude && this.wanderstein?.longitude) {
      const marker = L.marker([this.wanderstein.latitude, this.wanderstein.longitude]);
      marker.addTo(map);
      
      // Optionally add a popup
      const popupContent = `<strong>${this.escapeHtml(this.wanderstein.name)}</strong>`;
      marker.bindPopup(popupContent);
    }
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    const locale = this.languageService.getLanguage() === 'de' ? 'de-DE' : 'en-US';
    return date.toLocaleDateString(locale, {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  }

  goBack(): void {
    this.router.navigate(['/wandersteine']);
  }

  private escapeHtml(text: string): string {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
  }
}
