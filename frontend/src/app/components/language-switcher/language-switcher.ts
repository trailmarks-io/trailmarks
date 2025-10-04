import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LanguageService, SupportedLanguage } from '../../services/language';

@Component({
  selector: 'app-language-switcher',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './language-switcher.html',
  styleUrl: './language-switcher.css'
})
export class LanguageSwitcherComponent {
  supportedLanguages: SupportedLanguage[] = [];
  
  constructor(public languageService: LanguageService) {
    this.supportedLanguages = this.languageService.getSupportedLanguages();
  }

  switchLanguage(language: SupportedLanguage): void {
    this.languageService.setLanguage(language);
  }

  isCurrentLanguage(language: SupportedLanguage): boolean {
    return this.languageService.getLanguage() === language;
  }

  getLanguageLabel(language: SupportedLanguage): string {
    const labels: { [key in SupportedLanguage]: string } = {
      'de': 'DE',
      'en': 'EN'
    };
    return labels[language];
  }
}
