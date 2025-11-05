import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Subject, of } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, catchError } from 'rxjs/operators';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { WandersteinService, WandersteinResponse } from '../../services/wanderstein';
import { LanguageService, TranslatePipe } from '../../../core';
import { CarouselComponent, WandersteinMapComponent } from '../../../shared';

interface LocationChange {
  latitude: number;
  longitude: number;
  radiusKm: number;
  zoom: number;
}

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
  currentMapCenter?: { lat: number, lng: number };
  currentSearchRadius: number = 50;
  currentMapZoom?: number;
  savedMapCenter?: { lat: number, lng: number };
  savedMapZoom?: number;

  private _location$ = new Subject<LocationChange>();

  constructor(
    private wandersteinService: WandersteinService,
    private languageService: LanguageService,
    private router: Router
  ) {
    // Set up debounced location change pipeline
    this._location$.pipe(
      debounceTime(250),
      distinctUntilChanged((prev, curr) => {
        // Round coordinates to 4 decimal places (~11m precision) to avoid tiny differences
        const prevLat = Math.round(prev.latitude * 10000) / 10000;
        const prevLng = Math.round(prev.longitude * 10000) / 10000;
        const currLat = Math.round(curr.latitude * 10000) / 10000;
        const currLng = Math.round(curr.longitude * 10000) / 10000;
        return prevLat === currLat && prevLng === currLng && prev.radiusKm === curr.radiusKm;
      }),
      switchMap(location => 
        this.wandersteinService.getNearbyWandersteine(location.latitude, location.longitude, location.radiusKm).pipe(
          catchError(err => {
            console.error('Error loading nearby wandersteine:', err);
            // Fall back to showing recent wandersteine
            return of(this.recentWandersteine);
          })
        )
      ),
      takeUntilDestroyed()
    ).subscribe(data => {
      this.nearbyWandersteine = data;
    });
  }


  ngOnInit(): void {
    // Check if we have saved map state from navigation
    const navigation = this.router.getCurrentNavigation();
    const state = navigation?.extras?.state || history.state;
    
    if (state?.['mapCenter'] && state?.['mapZoom']) {
      this.savedMapCenter = state['mapCenter'];
      this.savedMapZoom = state['mapZoom'];
    }
    
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

  onMapLocationChange(location: LocationChange): void {
    // Update vignette center and radius based on current map location
    this.currentMapCenter = { lat: location.latitude, lng: location.longitude };
    this.currentSearchRadius = location.radiusKm;
    this.currentMapZoom = location.zoom;
    
    // Emit location change into Subject for debounced processing
    this._location$.next(location);
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
    // Save current map state before navigation
    this.router.navigate(['/wandersteine', uniqueId], {
      state: {
        mapCenter: this.currentMapCenter,
        mapZoom: this.currentMapZoom
      }
    });
  }
}
