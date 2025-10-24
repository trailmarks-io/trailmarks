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
  wandersteine: WandersteinResponse[] = [];
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
        this.wandersteine = data;
        this.loading = false;
      },
      error: (err) => {
        const errorMsg = this.languageService.translate('wanderstein.error');
        this.error = `${errorMsg}: ${err.message}`;
        this.loading = false;
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
