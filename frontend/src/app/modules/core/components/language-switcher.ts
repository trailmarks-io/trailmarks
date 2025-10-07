import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LanguageService, SupportedLanguage } from '../services/language';

@Component({
  selector: 'app-language-switcher',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './language-switcher.html'
})
export class LanguageSwitcherComponent {
  supportedLanguages: SupportedLanguage[] = [];
  
  constructor(public languageService: LanguageService) {
    this.supportedLanguages = this.languageService.getSupportedLanguages();
  }

  onLanguageChange(language: SupportedLanguage): void {
    this.languageService.setLanguage(language);
  }

  getLanguageLabel(language: SupportedLanguage): string {
    const labels: { [key in SupportedLanguage]: string } = {
      'de': 'Deutsch',
      'en': 'English'
    };
    return labels[language];
  }
}
