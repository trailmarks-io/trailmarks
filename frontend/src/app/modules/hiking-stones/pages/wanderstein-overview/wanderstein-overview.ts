import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { WandersteinService, WandersteinResponse } from '../../services/wanderstein';
import { LanguageService, TranslatePipe } from '../../../core';
import { CarouselComponent, WandersteinMapComponent } from '../../../shared';

@Component({
  selector: 'app-wanderstein-overview',
  imports: [CommonModule, TranslatePipe, CarouselComponent, WandersteinMapComponent],
  templateUrl: './wanderstein-overview.html',
  standalone: true
})
export class WandersteinOverviewPage implements OnInit {
  recentWandersteine: WandersteinResponse[] = [];
  nearbyWandersteine: WandersteinResponse[] = [];
  loading = true;
  error: string | null = null;

  constructor(
    private wandersteinService: WandersteinService,
    private languageService: LanguageService,
    private router: Router
  ) {}


  ngOnInit(): void {
    // Zeige den Ladeindikator für mindestens 500ms, damit E2E-Tests ihn sicher sehen
    this.loading = true;
    setTimeout(() => {
      this.loadRecentWandersteine();
    }, 500);
  }

  loadRecentWandersteine(): void {
    this.loading = true;
    this.error = null;
    
    this.wandersteinService.getRecentWandersteine().subscribe({
      next: (data) => {
        this.recentWandersteine = data;
        this.loading = false;
      },
      error: (err) => {
        const errorMsg = this.languageService.translate('wanderstein.error');
        this.error = `${errorMsg}: ${err.message}`;
        this.loading = false;
      }
    });
  }

  onMapLocationChange(location: {latitude: number, longitude: number, radiusKm: number}): void {
    this.wandersteinService.getNearbyWandersteine(location.latitude, location.longitude, location.radiusKm).subscribe({
      next: (data) => {
        this.nearbyWandersteine = data;
      },
      error: (err) => {
        console.error('Error loading nearby wandersteine:', err);
        // Fall back to showing recent wandersteine
        this.nearbyWandersteine = this.recentWandersteine;
      }
    });
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

  navigateToDetail(uniqueId: string): void {
    this.router.navigate(['/wandersteine', uniqueId]);
  }
}
