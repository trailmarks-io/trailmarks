import { LanguageService } from '../services/language';
import { TelemetryService } from '../services/telemetry.service';
import { environment } from '../../../../environments/environment';

export function initializeApp(languageService: LanguageService) {
  return () => {
    // Load translations before app starts
    const savedLanguage = localStorage.getItem('trailmarks_language') || 'de';
    languageService.currentLanguage.set(savedLanguage as 'de' | 'en');
    return languageService.loadTranslationsPromise(savedLanguage as 'de' | 'en');
  };
}

export function initializeTelemetry(telemetryService: TelemetryService) {
  return () => {
    // Initialize OpenTelemetry tracing
    telemetryService.initializeTracing(environment.otlpEndpoint);
  };
}
