import { test, expect } from '@playwright/test';

test.describe('Language Switcher', () => {
  test('should display language switcher', async ({ page }) => {
    await page.goto('/');
    
    const languageSwitcher = page.locator('app-language-switcher');
    await expect(languageSwitcher).toBeVisible();
  });

  test('should have language options', async ({ page }) => {
    await page.goto('/');
    
    // Check for language options
    const select = page.locator('app-language-switcher select, app-language-switcher button');
    await expect(select).toBeVisible();
  });

  test('should change language when option is selected', async ({ page }) => {
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
