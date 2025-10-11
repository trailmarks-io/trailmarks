import { test, expect } from '@playwright/test';

// TODO: Temporarily skipped - CI environment issues need to be resolved
// These tests work locally but fail in GitHub Actions CI
test.describe.skip('Header Component', () => {
  test.beforeEach(async ({ page }) => {
    // Mock translations API
    await page.route('**/api/translations/**', route => {
      route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({
          'wanderstein.title': 'Wandersteine',
          'wanderstein.subtitle': 'Entdecke die neuesten Wandersteine',
          'wanderstein.loading': 'Laden...',
          'wanderstein.error': 'Fehler beim Laden',
          'common.retry': 'Erneut versuchen'
        })
      });
    });

    // Mock wandersteine API
    await page.route('**/api/wandersteine/recent', route => {
      route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify([
          {
            id: 1,
            name: 'Test Stone',
            unique_Id: 'WS-001',
            preview_Url: 'https://via.placeholder.com/400',
            created_At: '2024-01-01T00:00:00Z'
          }
        ])
      });
    });
  });

  test.describe('Desktop View', () => {
    test.beforeEach(async ({ page }) => {
      await page.setViewportSize({ width: 1280, height: 720 });
    });

    test('should display header with Trailmarks title', async ({ page }) => {
      await page.goto('/');
      
      const header = page.locator('app-header');
      await expect(header).toBeVisible();
      
      const title = page.locator('app-header a[routerLink="/"]');
      await expect(title).toBeVisible();
      await expect(title).toHaveText('Trailmarks');
    });

    test('should have clickable Trailmarks logo linking to home', async ({ page }) => {
      await page.goto('/wandersteine');
      
      const titleLink = page.locator('app-header a[routerLink="/"]');
      await expect(titleLink).toHaveAttribute('href', '/');
    });

    test('should display language switcher in header', async ({ page }) => {
      await page.goto('/');
      
      // Language switcher should be visible in desktop view
      const languageSwitcher = page.locator('app-header .hidden.md\\:block app-language-switcher');
      await expect(languageSwitcher).toBeVisible();
    });

    test('should NOT display burger menu in desktop view', async ({ page }) => {
      await page.goto('/');
      
      // Burger menu should be hidden in desktop view
      const burgerButton = page.locator('button[aria-label="Toggle menu"]');
      await expect(burgerButton).toBeHidden();
    });

    test('should be fixed at the top', async ({ page }) => {
      await page.goto('/');
      
      const header = page.locator('header.fixed');
      await expect(header).toBeVisible();
      
      // Check if header has fixed positioning
      const headerElement = await header.elementHandle();
      const position = await headerElement?.evaluate(el => 
        window.getComputedStyle(el).position
      );
      expect(position).toBe('fixed');
    });
  });

  test.describe('Mobile View', () => {
    test.beforeEach(async ({ page }) => {
      await page.setViewportSize({ width: 375, height: 667 });
    });

    test('should display header with Trailmarks title', async ({ page }) => {
      await page.goto('/');
      
      const header = page.locator('app-header');
      await expect(header).toBeVisible();
      
      const title = page.locator('app-header a[routerLink="/"]');
      await expect(title).toBeVisible();
      await expect(title).toHaveText('Trailmarks');
    });

    test('should display burger menu button', async ({ page }) => {
      await page.goto('/');
      
      const burgerButton = page.locator('button[aria-label="Toggle menu"]');
      await expect(burgerButton).toBeVisible();
    });

    test('should NOT display language switcher in header on mobile', async ({ page }) => {
      await page.goto('/');
      
      // Desktop language switcher should be hidden on mobile
      const desktopLanguageSwitcher = page.locator('app-header .hidden.md\\:block app-language-switcher');
      await expect(desktopLanguageSwitcher).toBeHidden();
    });

    test('should open side navigation when burger button is clicked', async ({ page }) => {
      await page.goto('/');
      
      // Initially, side nav should not be visible
      let sideNav = page.locator('nav.fixed');
      await expect(sideNav).toBeHidden();
      
      // Click burger button
      const burgerButton = page.locator('button[aria-label="Toggle menu"]');
      await burgerButton.click();
      
      // Side nav should now be visible
      sideNav = page.locator('nav.fixed');
      await expect(sideNav).toBeVisible();
    });

    test('should display language switcher in side nav', async ({ page }) => {
      await page.goto('/');
      
      // Open side nav
      const burgerButton = page.locator('button[aria-label="Toggle menu"]');
      await burgerButton.click();
      
      // Language switcher should be visible in side nav
      const sideNavLanguageSwitcher = page.locator('nav.fixed app-language-switcher');
      await expect(sideNavLanguageSwitcher).toBeVisible();
    });

    test('should close side nav when backdrop is clicked', async ({ page }) => {
      await page.goto('/');
      
      // Open side nav
      const burgerButton = page.locator('button[aria-label="Toggle menu"]');
      await burgerButton.click();
      
      // Verify side nav is open
      let sideNav = page.locator('nav.fixed');
      await expect(sideNav).toBeVisible();
      
      // Click on backdrop (outside side nav)
      // Click at coordinates that would be on the backdrop but not on the side nav
      await page.mouse.click(50, 300);
      
      // Wait a bit for animation
      await page.waitForTimeout(500);
      
      // Side nav should be closed
      sideNav = page.locator('nav.fixed');
      await expect(sideNav).toBeHidden();
    });

    test('should show close icon when side nav is open', async ({ page }) => {
      await page.goto('/');
      
      // Open side nav
      const burgerButton = page.locator('button[aria-label="Toggle menu"]');
      await burgerButton.click();
      
      // Button should still be visible and show close icon
      await expect(burgerButton).toBeVisible();
      
      // You could verify the icon changed by checking SVG paths
      // The closed state has path "M4 6h16M4 12h16M4 18h16"
      // The open state has path "M6 18L18 6M6 6l12 12"
    });
  });

  test.describe('Responsive Behavior', () => {
    test('should adapt from desktop to mobile view', async ({ page }) => {
      // Start in desktop view
      await page.setViewportSize({ width: 1280, height: 720 });
      await page.goto('/');
      
      // Verify desktop state
      const desktopLanguageSwitcher = page.locator('app-header .hidden.md\\:block app-language-switcher');
      await expect(desktopLanguageSwitcher).toBeVisible();
      
      const burgerButton = page.locator('button[aria-label="Toggle menu"]');
      await expect(burgerButton).toBeHidden();
      
      // Resize to mobile
      await page.setViewportSize({ width: 375, height: 667 });
      
      // Wait for responsive changes
      await page.waitForTimeout(300);
      
      // Verify mobile state
      await expect(desktopLanguageSwitcher).toBeHidden();
      await expect(burgerButton).toBeVisible();
    });

    test('should adapt from mobile to desktop view', async ({ page }) => {
      // Start in mobile view
      await page.setViewportSize({ width: 375, height: 667 });
      await page.goto('/');
      
      // Open side nav
      const burgerButton = page.locator('button[aria-label="Toggle menu"]');
      await burgerButton.click();
      
      // Verify side nav is open
      let sideNav = page.locator('nav.fixed');
      await expect(sideNav).toBeVisible();
      
      // Resize to desktop
      await page.setViewportSize({ width: 1280, height: 720 });
      
      // Wait for responsive changes
      await page.waitForTimeout(300);
      
      // Verify desktop state
      const desktopLanguageSwitcher = page.locator('app-header .hidden.md\\:block app-language-switcher');
      await expect(desktopLanguageSwitcher).toBeVisible();
      await expect(burgerButton).toBeHidden();
      
      // Side nav should also be hidden (or not affect layout in desktop)
      // Note: The side nav might still be visible but is meant for mobile only
    });
  });

  test.describe('Navigation', () => {
    test('should navigate to home when title is clicked', async ({ page }) => {
      await page.goto('/wandersteine');
      
      // Click on Trailmarks title
      const titleLink = page.locator('app-header a[routerLink="/"]');
      await titleLink.click();
      
      // Should navigate to home page
      await expect(page).toHaveURL('/');
    });

    test('should remain visible when scrolling', async ({ page }) => {
      await page.goto('/');
      
      // Scroll down
      await page.evaluate(() => window.scrollBy(0, 500));
      
      // Header should still be visible (fixed position)
      const header = page.locator('app-header');
      await expect(header).toBeVisible();
    });
  });
});
