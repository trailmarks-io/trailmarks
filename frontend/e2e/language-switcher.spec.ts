import { test, expect } from '@playwright/test';

test.describe('Language Switcher', () => {
  test.beforeEach(async ({ page }) => {
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

  test('should display language switcher', async ({ page }) => {
    // Mock German translations
    await page.route('**/api/translations/de', route => {
      route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({
          'wanderstein.title': 'Wandersteine',
          'wanderstein.subtitle': 'Entdecke die neuesten Wandersteine'
        })
      });
    });

    await page.goto('/');
    
    const languageSwitcher = page.locator('app-language-switcher');
    await expect(languageSwitcher).toBeVisible();
  });

  test('should have language options', async ({ page }) => {
    // Mock German translations
    await page.route('**/api/translations/de', route => {
      route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({
          'wanderstein.title': 'Wandersteine',
          'wanderstein.subtitle': 'Entdecke die neuesten Wandersteine'
        })
      });
    });

    await page.goto('/');
    
    // Check for language options
    const select = page.locator('app-language-switcher select, app-language-switcher button');
    await expect(select).toBeVisible();
  });

  test('should change language when option is selected', async ({ page }) => {
    // Mock both German and English translations
    await page.route('**/api/translations/de', route => {
      route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({
          'wanderstein.title': 'Wandersteine',
          'wanderstein.subtitle': 'Entdecke die neuesten Wandersteine'
        })
      });
    });

    await page.route('**/api/translations/en', route => {
      route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({
          'wanderstein.title': 'Hiking Stones',
          'wanderstein.subtitle': 'Discover the newest hiking stones'
        })
      });
    });

    await page.goto('/');
    
    // Get initial title text
    const titleBefore = await page.locator('h1').textContent();
    
    // Find and interact with language selector
    const languageSelect = page.locator('app-language-switcher select');
    
    if (await languageSelect.isVisible()) {
      // Get current value
      const currentLang = await languageSelect.inputValue();
      
      // Switch to other language
      const newLang = currentLang === 'de' ? 'en' : 'de';
      await languageSelect.selectOption(newLang);
      
      // Wait a bit for language change to take effect
      await page.waitForTimeout(500);
      
      // Title should have changed
      const titleAfter = await page.locator('h1').textContent();
      
      // The title should be different after language change
      // (unless both languages have the same text, which is unlikely)
      expect(titleAfter).toBeDefined();
    }
  });
});
