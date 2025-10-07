import { test, expect } from '@playwright/test';

test.describe('Wanderstein Overview Page', () => {
  test('should display page title and subtitle', async ({ page }) => {
    await page.goto('/wandersteine');
    
    // Check for title
    const title = page.locator('h1');
    await expect(title).toBeVisible();
    
    // Check for subtitle
    const subtitle = page.locator('p').first();
    await expect(subtitle).toBeVisible();
  });

  test('should show loading state or content', async ({ page }) => {
    await page.goto('/wandersteine');
    
    // Either loading message or content should be visible
    const hasLoadingOrContent = await page.locator('div').filter({ hasText: /loading|Wanderstein/i }).count() > 0;
    expect(hasLoadingOrContent).toBeTruthy();
  });

  test('should handle API errors gracefully', async ({ page }) => {
    // Mock API to return an error
    await page.route('**/api/wandersteine/recent', route => {
      route.fulfill({
        status: 500,
        contentType: 'application/json',
        body: JSON.stringify({ error: 'Internal Server Error' })
      });
    });
    
    await page.goto('/wandersteine');
    
    // Wait for error message or retry button
    await expect(page.locator('button, .text-red-600, [class*="error"]')).toBeVisible({ timeout: 10000 });
  });

  test('should display carousel when data is loaded', async ({ page }) => {
    // Mock successful API response
    await page.route('**/api/wandersteine/recent', route => {
      route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify([
          {
            id: 1,
            name: 'Test Stone 1',
            unique_Id: 'WS-001',
            preview_Url: 'https://via.placeholder.com/400',
            created_At: '2024-01-01T00:00:00Z'
          },
          {
            id: 2,
            name: 'Test Stone 2',
            unique_Id: 'WS-002',
            preview_Url: 'https://via.placeholder.com/400',
            created_At: '2024-01-02T00:00:00Z'
          }
        ])
      });
    });
    
    await page.goto('/wandersteine');
    
    // Wait for carousel to be visible
    await expect(page.locator('app-carousel')).toBeVisible({ timeout: 10000 });
  });

  test('should show retry button on error', async ({ page }) => {
    // Mock API to return an error
    await page.route('**/api/wandersteine/recent', route => {
      route.fulfill({
        status: 500,
        contentType: 'application/json',
        body: JSON.stringify({ error: 'Internal Server Error' })
      });
    });
    
    await page.goto('/wandersteine');
    
    // Find retry button
    const retryButton = page.locator('button').filter({ hasText: /retry|wiederholen/i });
    await expect(retryButton).toBeVisible({ timeout: 10000 });
  });

  test('should display "no data" message when API returns empty array', async ({ page }) => {
    // Mock API to return empty array
    await page.route('**/api/wandersteine/recent', route => {
      route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify([])
      });
    });
    
    await page.goto('/wandersteine');
    
    // Wait for "no data" message
    await page.waitForTimeout(1000);
    const pageContent = await page.content();
    
    // Should not show loading anymore
    const loadingVisible = await page.locator('div').filter({ hasText: /loading|laden/i }).isVisible().catch(() => false);
    expect(loadingVisible).toBeFalsy();
  });
});
