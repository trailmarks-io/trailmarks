import { ComponentFixture, TestBed } from '@angular/core/testing';
import { WandersteinMapComponent } from './wanderstein-map';
import { WandersteinResponse } from '../../../hiking-stones';

describe('WandersteinMapComponent', () => {
  let component: WandersteinMapComponent;
  let fixture: ComponentFixture<WandersteinMapComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WandersteinMapComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(WandersteinMapComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with empty wandersteine array', () => {
    expect(component.wandersteine).toEqual([]);
  });

  it('should have default map options', () => {
    expect(component.options.zoom).toBe(2);
    expect(component.options.center).toBeDefined();
  });

  it('should not initialize markers until map is ready', () => {
    component.ngOnInit();
    expect(component.markers).toEqual([]);
  });

  it('should handle destroy when marker cluster group is null', () => {
    expect(() => component.ngOnDestroy()).not.toThrow();
  });

  it('should handle wandersteine with coordinates', () => {
    const testWandersteine: WandersteinResponse[] = [
      {
        id: 1,
        name: 'Test Stone',
        unique_Id: 'WS-001',
        preview_Url: 'test.jpg',
        created_At: '2024-01-01',
        latitude: 48.3019,
        longitude: 8.2392
      }
    ];
    
    component.wandersteine = testWandersteine;
    component.ngOnInit();
    
    // Markers are only created when map is ready
    expect(component.markers).toEqual([]);
  });

  it('should handle wandersteine without coordinates', () => {
    const testWandersteine: WandersteinResponse[] = [
      {
        id: 1,
        name: 'Test Stone',
        unique_Id: 'WS-001',
        preview_Url: 'test.jpg',
        created_At: '2024-01-01'
      }
    ];
    
    component.wandersteine = testWandersteine;
    component.ngOnInit();
    
    // Markers are only created when map is ready
    expect(component.markers).toEqual([]);
  });
});
