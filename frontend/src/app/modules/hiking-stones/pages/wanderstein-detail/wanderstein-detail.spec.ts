import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute, Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { WandersteinDetailPage } from './wanderstein-detail';
import { WandersteinService, WandersteinResponse } from '../../services/wanderstein';
import { LanguageService } from '../../../core';
import { provideHttpClient } from '@angular/common/http';

describe('WandersteinDetailPage', () => {
  let component: WandersteinDetailPage;
  let fixture: ComponentFixture<WandersteinDetailPage>;
  let wandersteinServiceSpy: jasmine.SpyObj<WandersteinService>;
  let languageServiceSpy: jasmine.SpyObj<LanguageService>;
  let routerSpy: jasmine.SpyObj<Router>;
  let activatedRouteSpy: jasmine.SpyObj<ActivatedRoute>;

  const mockWanderstein: WandersteinResponse = {
    id: 1,
    name: 'Test Stone',
    unique_Id: 'WS-TEST-001',
    preview_Url: 'https://example.com/test.jpg',
    created_At: '2024-01-15T10:30:00Z',
    latitude: 48.137154,
    longitude: 11.576124,
    location: 'Test Location Description'
  };

  beforeEach(async () => {
    wandersteinServiceSpy = jasmine.createSpyObj('WandersteinService', ['getWandersteinByUniqueId']);
    languageServiceSpy = jasmine.createSpyObj('LanguageService', ['translate', 'getLanguage']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    
    const paramMapSpy = jasmine.createSpyObj('ParamMap', ['get']);
    paramMapSpy.get.and.returnValue('WS-TEST-001');
    
    activatedRouteSpy = {
      snapshot: {
        paramMap: paramMapSpy
      }
    } as any;

    languageServiceSpy.translate.and.returnValue('Translated Text');
    languageServiceSpy.getLanguage.and.returnValue('en');

    await TestBed.configureTestingModule({
      imports: [WandersteinDetailPage],
      providers: [
        provideHttpClient(),
        { provide: WandersteinService, useValue: wandersteinServiceSpy },
        { provide: LanguageService, useValue: languageServiceSpy },
        { provide: Router, useValue: routerSpy },
        { provide: ActivatedRoute, useValue: activatedRouteSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(WandersteinDetailPage);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load wanderstein details on init', () => {
    wandersteinServiceSpy.getWandersteinByUniqueId.and.returnValue(of(mockWanderstein));

    component.ngOnInit();

    expect(wandersteinServiceSpy.getWandersteinByUniqueId).toHaveBeenCalledWith('WS-TEST-001');
    expect(component.wanderstein).toEqual(mockWanderstein);
    expect(component.loading).toBeFalse();
    expect(component.error).toBeNull();
  });

  it('should handle error when loading fails', () => {
    const errorResponse = { message: 'Not found' };
    wandersteinServiceSpy.getWandersteinByUniqueId.and.returnValue(throwError(() => errorResponse));

    component.ngOnInit();

    expect(component.loading).toBeFalse();
    expect(component.error).toContain('Not found');
  });

  it('should format date correctly', () => {
    wandersteinServiceSpy.getWandersteinByUniqueId.and.returnValue(of(mockWanderstein));
    component.ngOnInit();

    const formattedDate = component.formatDate('2024-01-15T10:30:00Z');
    expect(formattedDate).toBeTruthy();
    expect(formattedDate).toContain('2024');
  });

  it('should navigate back when goBack is called', () => {
    wandersteinServiceSpy.getWandersteinByUniqueId.and.returnValue(of(mockWanderstein));
    
    component.goBack();

    expect(routerSpy.navigate).toHaveBeenCalledWith(['/wandersteine']);
  });

  it('should update map center when coordinates are available', () => {
    wandersteinServiceSpy.getWandersteinByUniqueId.and.returnValue(of(mockWanderstein));

    component.ngOnInit();

    expect(component.mapOptions.center).toBeDefined();
  });

  it('should show error when uniqueId is not provided', async () => {
    const paramMapSpyNull = jasmine.createSpyObj('ParamMap', ['get']);
    paramMapSpyNull.get.and.returnValue(null);
    
    const newActivatedRoute = {
      snapshot: {
        paramMap: paramMapSpyNull
      }
    } as any;

    // Reconfigure TestBed for this specific test
    await TestBed.resetTestingModule();
    await TestBed.configureTestingModule({
      imports: [WandersteinDetailPage],
      providers: [
        provideHttpClient(),
        { provide: WandersteinService, useValue: wandersteinServiceSpy },
        { provide: LanguageService, useValue: languageServiceSpy },
        { provide: Router, useValue: routerSpy },
        { provide: ActivatedRoute, useValue: newActivatedRoute }
      ]
    }).compileComponents();

    const newFixture = TestBed.createComponent(WandersteinDetailPage);
    const newComponent = newFixture.componentInstance;
    
    newComponent.ngOnInit();

    expect(newComponent.loading).toBeFalse();
    expect(newComponent.error).toBeTruthy();
  });
});
