import { TestBed, ComponentFixture } from '@angular/core/testing';
import { of, throwError } from 'rxjs';
import { WandersteinOverviewComponent } from './wanderstein-overview';
import { WandersteinService, WandersteinResponse } from '../../services/wanderstein';
import { LanguageService } from '../../services/language';

describe('WandersteinOverviewComponent', () => {
  let component: WandersteinOverviewComponent;
  let fixture: ComponentFixture<WandersteinOverviewComponent>;
  let wandersteinServiceSpy: jasmine.SpyObj<WandersteinService>;
  let languageServiceSpy: jasmine.SpyObj<LanguageService>;

  beforeEach(async () => {
    wandersteinServiceSpy = jasmine.createSpyObj('WandersteinService', [
      'getRecentWandersteine'
    ]);
    
    languageServiceSpy = jasmine.createSpyObj('LanguageService', [
      'translate',
      'getLanguage',
      'getSupportedLanguages'
    ], {
      currentLanguage: jasmine.createSpy('currentLanguage').and.returnValue('de')
    });

    languageServiceSpy.getLanguage.and.returnValue('de');
    languageServiceSpy.translate.and.returnValue('Fehler beim Laden');
    languageServiceSpy.getSupportedLanguages.and.returnValue(['de', 'en']);

    await TestBed.configureTestingModule({
      imports: [WandersteinOverviewComponent],
      providers: [
        { provide: WandersteinService, useValue: wandersteinServiceSpy },
        { provide: LanguageService, useValue: languageServiceSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(WandersteinOverviewComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load wandersteine on init', () => {
    const mockData: WandersteinResponse[] = [
      {
        id: 1,
        name: 'Test Stone',
        unique_Id: 'WS-001',
        preview_Url: 'https://example.com/1.jpg',
        created_At: '2024-01-01T00:00:00Z'
      }
    ];

    wandersteinServiceSpy.getRecentWandersteine.and.returnValue(of(mockData));
    
    fixture.detectChanges(); // triggers ngOnInit
    
    expect(component.wandersteine).toEqual(mockData);
    expect(component.loading).toBeFalse();
    expect(component.error).toBeNull();
  });

  it('should set loading to true initially', () => {
    wandersteinServiceSpy.getRecentWandersteine.and.returnValue(of([]));
    
    expect(component.loading).toBeTrue();
  });

  it('should handle errors when loading wandersteine', () => {
    const errorResponse = { message: 'Server error' };
    wandersteinServiceSpy.getRecentWandersteine.and.returnValue(
      throwError(() => errorResponse)
    );
    
    fixture.detectChanges(); // triggers ngOnInit
    
    expect(component.error).toContain('Fehler beim Laden');
    expect(component.loading).toBeFalse();
  });

  it('should format dates correctly for German locale', () => {
    languageServiceSpy.getLanguage.and.returnValue('de');
    
    const dateString = '2024-01-15T10:30:00Z';
    const formatted = component.formatDate(dateString);
    
    expect(formatted).toContain('Januar');
    expect(formatted).toContain('2024');
  });

  it('should format dates correctly for English locale', () => {
    languageServiceSpy.getLanguage.and.returnValue('en');
    
    const dateString = '2024-01-15T10:30:00Z';
    const formatted = component.formatDate(dateString);
    
    expect(formatted).toContain('January');
    expect(formatted).toContain('2024');
  });

  it('should call loadRecentWandersteine on ngOnInit', () => {
    spyOn(component, 'loadRecentWandersteine');
    wandersteinServiceSpy.getRecentWandersteine.and.returnValue(of([]));
    
    component.ngOnInit();
    
    expect(component.loadRecentWandersteine).toHaveBeenCalled();
  });

  it('should set error to null when loading starts', () => {
    component.error = 'Previous error';
    wandersteinServiceSpy.getRecentWandersteine.and.returnValue(of([]));
    
    component.loadRecentWandersteine();
    
    // After observable completes
    expect(component.error).toBeNull();
  });

  it('should handle empty wandersteine list', () => {
    wandersteinServiceSpy.getRecentWandersteine.and.returnValue(of([]));
    
    fixture.detectChanges();
    
    expect(component.wandersteine).toEqual([]);
    expect(component.loading).toBeFalse();
  });

  it('should call translate with correct key on error', () => {
    const errorResponse = { message: 'Test error' };
    wandersteinServiceSpy.getRecentWandersteine.and.returnValue(
      throwError(() => errorResponse)
    );
    
    fixture.detectChanges();
    
    expect(languageServiceSpy.translate).toHaveBeenCalledWith('wanderstein.error');
  });

  it('should include error message in error property', () => {
    const errorResponse = { message: 'Network failure' };
    languageServiceSpy.translate.and.returnValue('Error loading');
    wandersteinServiceSpy.getRecentWandersteine.and.returnValue(
      throwError(() => errorResponse)
    );
    
    fixture.detectChanges();
    
    expect(component.error).toContain('Error loading');
    expect(component.error).toContain('Network failure');
  });
});
