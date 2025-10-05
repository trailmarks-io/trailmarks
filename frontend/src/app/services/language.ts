import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { environment } from '../../environments/environment';

export type SupportedLanguage = 'de' | 'en';

export interface Translation {
  [key: string]: string | Translation;
}

@Injectable({
  providedIn: 'root'
})
export class LanguageService {
  private readonly STORAGE_KEY = 'trailmarks_language';
  private readonly DEFAULT_LANGUAGE: SupportedLanguage = 'de';
  
  private translations: { [key: string]: Translation } = {};
  public currentLanguage = signal<SupportedLanguage>(this.DEFAULT_LANGUAGE);

  constructor(private http: HttpClient) {
    this.initializeLanguage();
  }

  private initializeLanguage(): void {
    const savedLanguage = localStorage.getItem(this.STORAGE_KEY) as SupportedLanguage | null;
    const language = savedLanguage || this.DEFAULT_LANGUAGE;
    this.currentLanguage.set(language);
    localStorage.setItem(this.STORAGE_KEY, language);
    // Load translations immediately
    this.loadTranslations(language).subscribe();
  }

  public setLanguage(language: SupportedLanguage): void {
    this.currentLanguage.set(language);
    localStorage.setItem(this.STORAGE_KEY, language);
    this.loadTranslations(language).subscribe();
  }

  public getLanguage(): SupportedLanguage {
    return this.currentLanguage();
  }

  private loadTranslations(language: SupportedLanguage): Observable<{ [key: string]: Translation }> {
    return this.http.get<{ [key: string]: Translation }>(`${environment.apiUrl}/api/translations/${language}`).pipe(
      map((translations) => {
        this.translations = translations;
        return translations;
      }),
      catchError((error) => {
        console.error(`Failed to load translations for ${language}:`, error);
        return of({});
      })
    );
  }

  public loadTranslationsPromise(language: SupportedLanguage): Promise<void> {
    return new Promise((resolve) => {
      this.loadTranslations(language).subscribe(() => {
        resolve();
      });
    });
  }

  public translate(key: string): string {
    const keys = key.split('.');
    let value: any = this.translations;
    
    for (const k of keys) {
      if (value && typeof value === 'object' && k in value) {
        value = value[k];
      } else {
        return key; // Return key if translation not found
      }
    }
    
    return typeof value === 'string' ? value : key;
  }

  public getSupportedLanguages(): SupportedLanguage[] {
    return ['de', 'en'];
  }
}
