import { TestBed } from '@angular/core/testing';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { LanguageService, SupportedLanguage } from './language';
import { provideHttpClient } from '@angular/common/http';

describe('LanguageService', () => {
  let service: LanguageService;
  let httpMock: HttpTestingController;
  let localStorageSpy: jasmine.SpyObj<Storage>;

  beforeEach(() => {
    // Create a spy for localStorage
    localStorageSpy = jasmine.createSpyObj('localStorage', ['getItem', 'setItem', 'clear']);
    localStorageSpy.getItem.and.returnValue(null); // Return null for fresh state
    
    // Replace the global localStorage with our spy
    Object.defineProperty(window, 'localStorage', { value: localStorageSpy, writable: true });

    TestBed.configureTestingModule({
      providers: [
        LanguageService,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });
    
    service = TestBed.inject(LanguageService);
    httpMock = TestBed.inject(HttpTestingController);
    
    // Flush initial translation loading request
    const initReq = httpMock.expectOne('http://localhost:8080/api/translations/de');
    initReq.flush({});
  });

  afterEach(() => {
    httpMock.verify();
    localStorageSpy.clear.calls.reset();
    localStorageSpy.getItem.calls.reset();
    localStorageSpy.setItem.calls.reset();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should initialize with default language', () => {
    expect(service.currentLanguage()).toBe('de');
  });

  it('should set language', () => {
    const mockTranslations = { 'test.key': 'Test Value' };
    
    service.setLanguage('en');
    
    const req = httpMock.expectOne('http://localhost:8080/api/translations/en');
    expect(req.request.method).toBe('GET');
    req.flush(mockTranslations);
    
    expect(service.currentLanguage()).toBe('en');
  });

  it('should get current language', () => {
    expect(service.getLanguage()).toBe('de');
  });

  it('should return supported languages', () => {
    const languages = service.getSupportedLanguages();
    expect(languages).toEqual(['de', 'en']);
  });

  it('should translate keys', (done) => {
    const mockTranslations = { 
      app: { 
        title: 'Trailmarks' 
      }
    };
    
    service.setLanguage('de');
    
    const req = httpMock.expectOne('http://localhost:8080/api/translations/de');
    req.flush(mockTranslations);
    
    setTimeout(() => {
      const translated = service.translate('app.title');
      expect(translated).toBe('Trailmarks');
      done();
    }, 100);
  });

  it('should return key when translation not found', (done) => {
    const mockTranslations = { test: { key: 'value' } };
    
    service.setLanguage('de');
    
    const req = httpMock.expectOne('http://localhost:8080/api/translations/de');
    req.flush(mockTranslations);
    
    setTimeout(() => {
      const translated = service.translate('nonexistent.key');
      expect(translated).toBe('nonexistent.key');
      done();
    }, 100);
  });

  it('should handle nested translation keys', (done) => {
    const mockTranslations = { 
      level1: { 
        level2: { 
          level3: 'Deep Value' 
        } 
      }
    };
    
    service.setLanguage('en');
    
    const req = httpMock.expectOne('http://localhost:8080/api/translations/en');
    req.flush(mockTranslations);
    
    setTimeout(() => {
      const translated = service.translate('level1.level2.level3');
      expect(translated).toBe('Deep Value');
      done();
    }, 100);
  });

  it('should handle HTTP errors gracefully', (done) => {
    service.setLanguage('en');
    
    const req = httpMock.expectOne('http://localhost:8080/api/translations/en');
    req.flush('Error', { status: 500, statusText: 'Internal Server Error' });
    
    setTimeout(() => {
      // Service should still work, just with no translations
      expect(service.currentLanguage()).toBe('en');
      done();
    }, 100);
  });

  it('should load translations as promise', async () => {
    const mockTranslations = { test: { key: 'value' } };
    
    const promise = service.loadTranslationsPromise('de');
    
    const req = httpMock.expectOne('http://localhost:8080/api/translations/de');
    req.flush(mockTranslations);
    
    await expectAsync(promise).toBeResolved();
  });
});
