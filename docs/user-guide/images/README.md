# User Guide Screenshots

This directory contains screenshots for the Trailmarks user guide documentation.

## Required Screenshots

The following screenshots need to be captured from the running application:

### Desktop Views

1. **homepage-desktop.png**
   - Full homepage view showing header, title, and carousel
   - Should show 3 hiking stone cards in the carousel
   - Language switcher visible in top-right corner
   - Recommended size: 1920x1080 or similar

2. **hiking-stone-cards-desktop.png**
   - Close-up of hiking stone cards showing:
     - Preview image
     - Stone name
     - Unique ID (e.g., WS-2024-001)
     - Date added
   - Should show card hover effect if possible

3. **carousel-navigation-desktop.png**
   - Carousel with visible navigation arrows (previous/next buttons)
   - Pagination indicators at the bottom
   - Multiple cards visible

4. **language-switcher-desktop.png**
   - Close-up of the language switcher dropdown in the header
   - Show both closed and open states if possible

### Mobile Views

5. **homepage-mobile.png**
   - Full homepage view on mobile device (375x667 or similar)
   - Should show responsive layout with single column
   - Burger menu visible in top-left corner

6. **mobile-menu-closed.png**
   - Mobile header with burger menu button (closed state)
   - Show the three horizontal lines icon

7. **mobile-menu-open.png**
   - Mobile side navigation panel open
   - Language switcher visible in the side panel
   - Show the backdrop/overlay

8. **carousel-mobile.png**
   - Carousel on mobile showing single card view
   - Navigation indicators visible

### Optional Screenshots

9. **error-state.png**
   - Error message display with retry button

10. **loading-state.png**
    - Loading indicator shown while data is being fetched

## Screenshot Guidelines

- **Format**: PNG with good quality
- **Desktop Resolution**: 1920x1080 or 1440x900
- **Mobile Resolution**: 375x667 (iPhone SE) or 412x915 (Pixel 5)
- **Browser**: Use Chrome or Firefox for consistency
- **Clean State**: Ensure no browser extensions or dev tools visible
- **Actual Data**: Use real data from the running application
- **Naming Convention**: Use descriptive, lowercase names with hyphens

## How to Take Screenshots

1. Start the application:
   ```bash
   docker compose up -d
   ```

2. Wait for services to be ready (check http://localhost:4200)

3. For desktop screenshots:
   - Open browser at 1920x1080 resolution
   - Navigate to http://localhost:4200
   - Take full-page or specific component screenshots

4. For mobile screenshots:
   - Use browser DevTools device emulation
   - Select iPhone SE or similar mobile device
   - Take screenshots in mobile view

## Embedding in Documentation

Screenshots are embedded in the AsciiDoc documentation using:

```asciidoc
image::images/filename.png[Alt text describing the image, width=800]
```

Or with a caption:

```asciidoc
.Caption describing the screenshot
image::images/filename.png[Alt text, width=800]
```
