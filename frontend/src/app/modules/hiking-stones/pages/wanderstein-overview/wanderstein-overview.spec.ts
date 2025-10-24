import { TestBed, ComponentFixture, fakeAsync, tick } from '@angular/core/testing';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { WandersteinOverviewPage } from './wanderstein-overview';
import { WandersteinService, WandersteinResponse } from '../../services/wanderstein';
import { LanguageService } from '../../../core';

describe('WandersteinOverviewPage', () => {
  let component: WandersteinOverviewPage;
  let fixture: ComponentFixture<WandersteinOverviewPage>;
  let wandersteinServiceSpy: jasmine.SpyObj<WandersteinService>;
  let languageServiceSpy: jasmine.SpyObj<LanguageService>;
  let routerSpy: jasmine.SpyObj<Router>;

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

    routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    languageServiceSpy.getLanguage.and.returnValue('de');
    languageServiceSpy.translate.and.returnValue('Fehler beim Laden');
    languageServiceSpy.getSupportedLanguages.and.returnValue(['de', 'en']);

    await TestBed.configureTestingModule({
      imports: [WandersteinOverviewPage],
      providers: [
        { provide: WandersteinService, useValue: wandersteinServiceSpy },
        { provide: LanguageService, useValue: languageServiceSpy },
        { provide: Router, useValue: routerSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(WandersteinOverviewPage);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load wandersteine on init', fakeAsync(() => {
    const mockData: WandersteinResponse[] = [
      {
        id: 1,
        name: 'Test Stone',
        unique_Id: 'WS-001',
        preview_Url: 'https://example.com/1.jpg',
        created_At: '2024-01-01T00:00:00Z',
        location: 'Test Location'
      }
    ];

    wandersteinServiceSpy.getRecentWandersteine.and.returnValue(of(mockData));
    
  fixture.detectChanges(); // triggers ngOnInit
  tick(500);
    
    expect(component.wandersteine).toEqual(mockData);
    expect(component.loading).toBeFalse();
    expect(component.error).toBeNull();
  }));

  it('should set loading to true initially', () => {
    wandersteinServiceSpy.getRecentWandersteine.and.returnValue(of([]));
    
    expect(component.loading).toBeTrue();
  });

  it('should handle errors when loading wandersteine', fakeAsync(() => {
    const errorResponse = { message: 'Server error' };
    wandersteinServiceSpy.getRecentWandersteine.and.returnValue(
      throwError(() => errorResponse)
    );
    
  fixture.detectChanges(); // triggers ngOnInit
  tick(500);
    
    expect(component.error).toContain('Fehler beim Laden');
    expect(component.loading).toBeFalse();
  }));

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

  it('should navigate to detail page when navigateToDetail is called', () => {
    component.navigateToDetail('WS-TEST-001');

    expect(routerSpy.navigate).toHaveBeenCalledWith(['/wandersteine', 'WS-TEST-001']);
  });

  it('should call loadRecentWandersteine on ngOnInit', fakeAsync(() => {
    spyOn(component, 'loadRecentWandersteine');
    wandersteinServiceSpy.getRecentWandersteine.and.returnValue(of([]));
    component.ngOnInit();
    tick(500);
    expect(component.loadRecentWandersteine).toHaveBeenCalled();
  }));

  it('should set error to null when loading starts', () => {
    component.error = 'Previous error';
    wandersteinServiceSpy.getRecentWandersteine.and.returnValue(of([]));
    
    component.loadRecentWandersteine();
    
    // After observable completes
    expect(component.error).toBeNull();
  });

  it('should handle empty wandersteine list', fakeAsync(() => {
    wandersteinServiceSpy.getRecentWandersteine.and.returnValue(of([]));
    
  fixture.detectChanges();
  tick(500);
    
    expect(component.wandersteine).toEqual([]);
    expect(component.loading).toBeFalse();
  }));

  it('should call translate with correct key on error', fakeAsync(() => {
    const errorResponse = { message: 'Test error' };
    wandersteinServiceSpy.getRecentWandersteine.and.returnValue(
      throwError(() => errorResponse)
    );
    
  fixture.detectChanges();
  tick(500);
    
    expect(languageServiceSpy.translate).toHaveBeenCalledWith('wanderstein.error');
  }));

  it('should include error message in error property', fakeAsync(() => {
    const errorResponse = { message: 'Network failure' };
    languageServiceSpy.translate.and.returnValue('Error loading');
    wandersteinServiceSpy.getRecentWandersteine.and.returnValue(
      throwError(() => errorResponse)
    );
    
  fixture.detectChanges();
  tick(500);
    
    expect(component.error).toContain('Error loading');
    expect(component.error).toContain('Network failure');
  }));
});
