import { TestBed, ComponentFixture } from '@angular/core/testing';
import { LanguageSwitcherComponent } from './language-switcher';
import { LanguageService, SupportedLanguage } from '../services/language';

describe('LanguageSwitcherComponent', () => {
  let component: LanguageSwitcherComponent;
  let fixture: ComponentFixture<LanguageSwitcherComponent>;
  let languageServiceSpy: jasmine.SpyObj<LanguageService>;

  beforeEach(async () => {
    languageServiceSpy = jasmine.createSpyObj('LanguageService', [
      'setLanguage',
      'getSupportedLanguages'
    ], {
      currentLanguage: jasmine.createSpy('currentLanguage').and.returnValue('de')
    });
    
    languageServiceSpy.getSupportedLanguages.and.returnValue(['de', 'en']);

    await TestBed.configureTestingModule({
      imports: [LanguageSwitcherComponent],
      providers: [
        { provide: LanguageService, useValue: languageServiceSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(LanguageSwitcherComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load supported languages on init', () => {
    expect(component.supportedLanguages).toEqual(['de', 'en']);
    expect(languageServiceSpy.getSupportedLanguages).toHaveBeenCalled();
  });

  it('should call setLanguage when language changes', () => {
    component.onLanguageChange('en');
    
    expect(languageServiceSpy.setLanguage).toHaveBeenCalledWith('en');
  });

  it('should return correct language label for German', () => {
    const label = component.getLanguageLabel('de');
    expect(label).toBe('Deutsch');
  });

  it('should return correct language label for English', () => {
    const label = component.getLanguageLabel('en');
    expect(label).toBe('English');
  });

  it('should have languageService injected', () => {
    expect(component.languageService).toBe(languageServiceSpy);
  });

  it('should handle multiple language changes', () => {
    component.onLanguageChange('en');
    component.onLanguageChange('de');
    component.onLanguageChange('en');
    
    expect(languageServiceSpy.setLanguage).toHaveBeenCalledTimes(3);
    expect(languageServiceSpy.setLanguage).toHaveBeenCalledWith('en');
    expect(languageServiceSpy.setLanguage).toHaveBeenCalledWith('de');
  });
});
