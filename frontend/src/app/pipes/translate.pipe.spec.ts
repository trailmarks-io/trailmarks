import { TestBed } from '@angular/core/testing';
import { ChangeDetectorRef, Injector } from '@angular/core';
import { TranslatePipe } from './translate.pipe';
import { LanguageService } from '../services/language';

describe('TranslatePipe', () => {
  let pipe: TranslatePipe;
  let languageServiceSpy: jasmine.SpyObj<LanguageService>;
  let changeDetectorRefSpy: jasmine.SpyObj<ChangeDetectorRef>;
  let injector: Injector;

  beforeEach(() => {
    languageServiceSpy = jasmine.createSpyObj('LanguageService', [
      'translate',
      'getLanguage'
    ], {
      currentLanguage: jasmine.createSpy('currentLanguage').and.returnValue('de')
    });
    
    changeDetectorRefSpy = jasmine.createSpyObj('ChangeDetectorRef', ['markForCheck']);

    TestBed.configureTestingModule({
      providers: [
        { provide: LanguageService, useValue: languageServiceSpy },
        { provide: ChangeDetectorRef, useValue: changeDetectorRefSpy }
      ]
    });
    
    injector = TestBed.inject(Injector);
    languageServiceSpy.getLanguage.and.returnValue('de');
    pipe = new TranslatePipe(languageServiceSpy, changeDetectorRefSpy, injector);
  });

  it('should create', () => {
    expect(pipe).toBeTruthy();
  });

  it('should transform key to translated value', () => {
    languageServiceSpy.translate.and.returnValue('Übersetzter Text');
    
    const result = pipe.transform('app.title');
    
    expect(result).toBe('Übersetzter Text');
    expect(languageServiceSpy.translate).toHaveBeenCalledWith('app.title');
  });

  it('should cache translation results', () => {
    languageServiceSpy.translate.and.returnValue('Cached Value');
    
    // First call
    pipe.transform('test.key');
    expect(languageServiceSpy.translate).toHaveBeenCalledTimes(1);
    
    // Second call with same key and language
    pipe.transform('test.key');
    expect(languageServiceSpy.translate).toHaveBeenCalledTimes(1); // Should not call again
  });

  it('should re-translate when key changes', () => {
    languageServiceSpy.translate.and.returnValues('First', 'Second');
    
    pipe.transform('key1');
    expect(languageServiceSpy.translate).toHaveBeenCalledWith('key1');
    
    pipe.transform('key2');
    expect(languageServiceSpy.translate).toHaveBeenCalledWith('key2');
    expect(languageServiceSpy.translate).toHaveBeenCalledTimes(2);
  });

  it('should re-translate when language changes', () => {
    languageServiceSpy.translate.and.returnValues('Deutsch', 'English');
    languageServiceSpy.getLanguage.and.returnValues('de', 'en');
    
    const firstResult = pipe.transform('test.key');
    expect(firstResult).toBe('Deutsch');
    
    // Simulate language change
    const secondResult = pipe.transform('test.key');
    expect(languageServiceSpy.translate).toHaveBeenCalledTimes(2);
  });

  it('should return key if translation not found', () => {
    languageServiceSpy.translate.and.returnValue('missing.key');
    
    const result = pipe.transform('missing.key');
    
    expect(result).toBe('missing.key');
  });

  it('should handle empty string keys', () => {
    languageServiceSpy.translate.and.returnValue('');
    
    const result = pipe.transform('');
    
    expect(result).toBe('');
    expect(languageServiceSpy.translate).toHaveBeenCalledWith('');
  });
});
