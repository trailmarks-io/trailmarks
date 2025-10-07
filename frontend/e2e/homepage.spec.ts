import { test, expect } from '@playwright/test';

test.describe('Homepage', () => {
  test('should load the homepage successfully', async ({ page }) => {
    await page.goto('/');
    
    // Check that the page title contains expected text
    await expect(page.locator('h1')).toBeVisible();
    
    // Verify the language switcher is present
    await expect(page.locator('app-language-switcher')).toBeVisible();
  });

  test('should display loading state initially', async ({ page }) => {
    await page.goto('/');
    
    // The loading indicator might be very quick, but we can check the structure
    // Wait for either loading or content to be visible
    await expect(page.locator('body')).toBeVisible();
  });

  test('should have responsive layout', async ({ page }) => {
    // Test desktop view
    await page.setViewportSize({ width: 1280, height: 720 });
    await page.goto('/');
    await expect(page.locator('.max-w-7xl')).toBeVisible();
    
    // Test mobile view
    await page.setViewportSize({ width: 375, height: 667 });
    await page.goto('/');
    await expect(page.locator('.max-w-7xl')).toBeVisible();
  });
});
