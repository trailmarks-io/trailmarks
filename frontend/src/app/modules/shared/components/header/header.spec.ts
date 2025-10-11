import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HeaderComponent } from './header';
import { LanguageService } from '../../../core';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';

describe('HeaderComponent', () => {
  let component: HeaderComponent;
  let fixture: ComponentFixture<HeaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HeaderComponent],
      providers: [
        provideRouter([]),
        provideHttpClient(),
        provideHttpClientTesting(),
        LanguageService
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(HeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display Trailmarks title', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    const titleElement = compiled.querySelector('a[routerLink="/"]');
    expect(titleElement?.textContent?.trim()).toBe('Trailmarks');
  });

  it('should have language switcher in desktop view', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    const desktopLanguageSwitcher = compiled.querySelector('.hidden.md\\:block app-language-switcher');
    expect(desktopLanguageSwitcher).toBeTruthy();
  });

  it('should have burger menu button', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    const burgerButton = compiled.querySelector('button[aria-label="Toggle menu"]');
    expect(burgerButton).toBeTruthy();
  });

  it('should toggle side nav when burger button is clicked', () => {
    expect(component.isSideNavOpen()).toBe(false);
    
    component.toggleSideNav();
    expect(component.isSideNavOpen()).toBe(true);
    
    component.toggleSideNav();
    expect(component.isSideNavOpen()).toBe(false);
  });

  it('should close side nav when closeSideNav is called', () => {
    component.isSideNavOpen.set(true);
    expect(component.isSideNavOpen()).toBe(true);
    
    component.closeSideNav();
    expect(component.isSideNavOpen()).toBe(false);
  });

  it('should show side nav when isSideNavOpen is true', () => {
    component.isSideNavOpen.set(true);
    fixture.detectChanges();
    
    const compiled = fixture.nativeElement as HTMLElement;
    const sideNav = compiled.querySelector('nav.fixed');
    expect(sideNav).toBeTruthy();
  });

  it('should hide side nav when isSideNavOpen is false', () => {
    component.isSideNavOpen.set(false);
    fixture.detectChanges();
    
    const compiled = fixture.nativeElement as HTMLElement;
    const sideNav = compiled.querySelector('nav.fixed');
    expect(sideNav).toBeFalsy();
  });

  it('should have language switcher in side nav', () => {
    component.isSideNavOpen.set(true);
    fixture.detectChanges();
    
    const compiled = fixture.nativeElement as HTMLElement;
    const sideNavLanguageSwitcher = compiled.querySelector('nav.fixed app-language-switcher');
    expect(sideNavLanguageSwitcher).toBeTruthy();
  });
});
