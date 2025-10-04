import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WandersteinService, WandersteinResponse } from '../../services/wanderstein';
import { LanguageService } from '../../services/language';
import { TranslatePipe } from '../../pipes/translate.pipe';
import { LanguageSwitcherComponent } from '../language-switcher/language-switcher';

@Component({
  selector: 'app-wanderstein-overview',
  imports: [CommonModule, TranslatePipe, LanguageSwitcherComponent],
  templateUrl: './wanderstein-overview.html',
  standalone: true
})
export class WandersteinOverviewComponent implements OnInit {
  wandersteine: WandersteinResponse[] = [];
  loading = true;
  error: string | null = null;

  constructor(
    private wandersteinService: WandersteinService,
    private languageService: LanguageService
  ) {}

  ngOnInit(): void {
    this.loadRecentWandersteine();
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
}
