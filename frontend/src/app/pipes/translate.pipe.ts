import { Pipe, PipeTransform, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { LanguageService } from '../services/language';
import { effect, Injector } from '@angular/core';

@Pipe({
  name: 'translate',
  pure: false,
  standalone: true
})
export class TranslatePipe implements PipeTransform, OnDestroy {
  private lastValue: string = '';
  private lastKey: string = '';
  private lastLanguage: string = '';

  constructor(
    private languageService: LanguageService,
    private cdr: ChangeDetectorRef,
    private injector: Injector
  ) {
    // Listen for language changes and trigger change detection
    effect(() => {
      this.languageService.currentLanguage();
      this.lastLanguage = ''; // Reset to force re-translation
      this.cdr.markForCheck();
    }, { injector: this.injector });
  }

  transform(key: string): string {
    const currentLanguage = this.languageService.getLanguage();
    if (key !== this.lastKey || currentLanguage !== this.lastLanguage) {
      this.lastKey = key;
      this.lastLanguage = currentLanguage;
      this.lastValue = this.languageService.translate(key);
    }
    return this.lastValue;
  }

  ngOnDestroy(): void {
    // Cleanup is handled automatically by Angular's effect
  }
}
