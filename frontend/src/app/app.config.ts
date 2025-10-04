import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZoneChangeDetection, APP_INITIALIZER } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { LanguageService } from './services/language';

import { routes } from './app.routes';

export function initializeApp(languageService: LanguageService) {
  return () => {
    // Load translations before app starts
    const savedLanguage = localStorage.getItem('trailmarks_language') || 'de';
    languageService.currentLanguage.set(savedLanguage as 'de' | 'en');
    return languageService.loadTranslationsPromise(savedLanguage as 'de' | 'en');
  };
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(),
    {
      provide: APP_INITIALIZER,
      useFactory: initializeApp,
      deps: [LanguageService],
      multi: true
    }
  ]
};
