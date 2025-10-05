import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CarouselComponent } from './carousel';

describe('CarouselComponent', () => {
  let component: CarouselComponent;
  let fixture: ComponentFixture<CarouselComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CarouselComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(CarouselComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with default values', () => {
    expect(component.items).toEqual([]);
    expect(component.maxVisibleItems).toBe(3);
    expect(component.currentIndex).toBe(0);
  });

  it('should return correct visible items', () => {
    component.items = [1, 2, 3, 4, 5];
    component.maxVisibleItems = 3;
    component.currentIndex = 0;

    expect(component.visibleItems).toEqual([1, 2, 3]);
  });

  it('should return correct visible items when at end', () => {
    component.items = [1, 2, 3, 4, 5];
    component.maxVisibleItems = 3;
    component.currentIndex = 3;

    expect(component.visibleItems).toEqual([4, 5]);
  });

  it('should detect when can go previous', () => {
    component.currentIndex = 0;
    expect(component.canGoPrevious).toBeFalse();

    component.currentIndex = 1;
    expect(component.canGoPrevious).toBeTrue();
  });

  it('should detect when can go next', () => {
    component.items = [1, 2, 3, 4, 5];
    component.maxVisibleItems = 3;
    component.currentIndex = 0;

    expect(component.canGoNext).toBeTrue();

    component.currentIndex = 2;
    expect(component.canGoNext).toBeFalse();
  });

  it('should go to previous item', () => {
    component.currentIndex = 2;
    component.goToPrevious();

    expect(component.currentIndex).toBe(1);
  });

  it('should not go to previous when at start', () => {
    component.currentIndex = 0;
    component.goToPrevious();

    expect(component.currentIndex).toBe(0);
  });

  it('should go to next item', () => {
    component.items = [1, 2, 3, 4, 5];
    component.maxVisibleItems = 3;
    component.currentIndex = 0;

    component.goToNext();

    expect(component.currentIndex).toBe(1);
  });

  it('should not go to next when at end', () => {
    component.items = [1, 2, 3, 4, 5];
    component.maxVisibleItems = 3;
    component.currentIndex = 2;

    component.goToNext();

    expect(component.currentIndex).toBe(2);
  });

  it('should handle touch swipe left (next)', () => {
    component.items = [1, 2, 3, 4, 5];
    component.maxVisibleItems = 3;
    component.currentIndex = 0;

    const touchStartEvent = {
      changedTouches: [{ screenX: 200 }]
    } as unknown as TouchEvent;
    const touchEndEvent = {
      changedTouches: [{ screenX: 100 }]
    } as unknown as TouchEvent;

    component.onTouchStart(touchStartEvent);
    component.onTouchEnd(touchEndEvent);

    expect(component.currentIndex).toBe(1);
  });

  it('should handle touch swipe right (previous)', () => {
    component.items = [1, 2, 3, 4, 5];
    component.maxVisibleItems = 3;
    component.currentIndex = 1;

    const touchStartEvent = {
      changedTouches: [{ screenX: 100 }]
    } as unknown as TouchEvent;
    const touchEndEvent = {
      changedTouches: [{ screenX: 200 }]
    } as unknown as TouchEvent;

    component.onTouchStart(touchStartEvent);
    component.onTouchEnd(touchEndEvent);

    expect(component.currentIndex).toBe(0);
  });

  it('should not swipe if movement is below threshold', () => {
    component.items = [1, 2, 3, 4, 5];
    component.maxVisibleItems = 3;
    component.currentIndex = 0;

    const touchStartEvent = {
      changedTouches: [{ screenX: 100 }]
    } as unknown as TouchEvent;
    const touchEndEvent = {
      changedTouches: [{ screenX: 90 }]
    } as unknown as TouchEvent;

    component.onTouchStart(touchStartEvent);
    component.onTouchEnd(touchEndEvent);

    expect(component.currentIndex).toBe(0);
  });

  it('should handle different maxVisibleItems values', () => {
    component.items = [1, 2, 3, 4, 5];
    component.maxVisibleItems = 2;
    component.currentIndex = 0;

    expect(component.visibleItems).toEqual([1, 2]);
    expect(component.canGoNext).toBeTrue();

    component.currentIndex = 3;
    expect(component.canGoNext).toBeFalse();
  });
});
