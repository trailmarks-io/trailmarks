import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SimpleChange } from '@angular/core';
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

  it('should not initialize marker cluster until map is ready', () => {
    component.ngOnInit();
    expect(component.markerClusterGroup).toBeNull();
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
    
    // Marker cluster is only created when map is ready
    expect(component.markerClusterGroup).toBeNull();
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
    
    // Marker cluster is only created when map is ready
    expect(component.markerClusterGroup).toBeNull();
  });

  it('should not refresh markers on first change', () => {
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

    // Set up map and marker cluster to simulate ready state
    const mockMarkerCluster = {
      clearLayers: jasmine.createSpy('clearLayers'),
      addLayer: jasmine.createSpy('addLayer'),
      getBounds: jasmine.createSpy('getBounds').and.returnValue({
        isValid: () => false
      }),
      off: jasmine.createSpy('off')
    };
    
    component.map = {
      removeLayer: jasmine.createSpy('removeLayer')
    } as any;
    component.markerClusterGroup = mockMarkerCluster;

    // First change should not trigger refresh
    component.ngOnChanges({
      wandersteine: new SimpleChange(undefined, testWandersteine, true)
    });

    expect(mockMarkerCluster.clearLayers).not.toHaveBeenCalled();
  });

  it('should refresh markers on subsequent changes when map is ready', () => {
    const initialWandersteine: WandersteinResponse[] = [
      {
        id: 1,
        name: 'Test Stone 1',
        unique_Id: 'WS-001',
        preview_Url: 'test1.jpg',
        created_At: '2024-01-01',
        latitude: 48.3019,
        longitude: 8.2392
      }
    ];

    const updatedWandersteine: WandersteinResponse[] = [
      {
        id: 2,
        name: 'Test Stone 2',
        unique_Id: 'WS-002',
        preview_Url: 'test2.jpg',
        created_At: '2024-01-02',
        latitude: 49.0,
        longitude: 9.0
      }
    ];

    // Set up map and marker cluster to simulate ready state
    const mockMarkerCluster = {
      clearLayers: jasmine.createSpy('clearLayers'),
      addLayer: jasmine.createSpy('addLayer'),
      getBounds: jasmine.createSpy('getBounds').and.returnValue({
        isValid: () => true,
        pad: (value: number) => ({})
      }),
      off: jasmine.createSpy('off')
    };
    
    component.map = {
      fitBounds: jasmine.createSpy('fitBounds'),
      removeLayer: jasmine.createSpy('removeLayer')
    } as any;
    
    component.markerClusterGroup = mockMarkerCluster;
    component.wandersteine = updatedWandersteine;

    // Subsequent change should trigger refresh
    component.ngOnChanges({
      wandersteine: new SimpleChange(initialWandersteine, updatedWandersteine, false)
    });

    expect(mockMarkerCluster.clearLayers).toHaveBeenCalled();
    expect(mockMarkerCluster.addLayer).toHaveBeenCalled();
  });

  it('should not refresh markers when map is not ready', () => {
    const initialWandersteine: WandersteinResponse[] = [];
    const updatedWandersteine: WandersteinResponse[] = [
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

    // Map not ready - no markerClusterGroup
    component.map = null;
    component.markerClusterGroup = null;
    component.wandersteine = updatedWandersteine;

    // Should not throw error even when not ready
    expect(() => {
      component.ngOnChanges({
        wandersteine: new SimpleChange(initialWandersteine, updatedWandersteine, false)
      });
    }).not.toThrow();
  });

  it('should clean up event listeners on destroy', () => {
    const mockMarkerCluster = {
      off: jasmine.createSpy('off')
    };
    
    component.markerClusterGroup = mockMarkerCluster;
    component.map = {
      removeLayer: jasmine.createSpy('removeLayer')
    } as any;
    component['clusteringEndListener'] = jasmine.createSpy('listener');

    component.ngOnDestroy();

    expect(mockMarkerCluster.off).toHaveBeenCalledWith('animationend', jasmine.any(Function));
    expect(component['clusteringEndListener']).toBeNull();
  });
});
