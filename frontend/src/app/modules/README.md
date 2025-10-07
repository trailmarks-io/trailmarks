# Angular Module Structure

This directory contains the modular organization of the Trailmarks Angular application.

## Module Overview

### Core Module (`/core`)
Infrastructure and cross-cutting concerns that are essential for the application.

**Contents**:
- **Services**: Language management, Telemetry/OpenTelemetry integration, Translation pipe
- **Components**: Language switcher component
- **Initializers**: App initialization functions (language, telemetry)

**Purpose**: Provides foundational services and components used throughout the application.

### Shared Module (`/shared`)
Reusable UI components without domain-specific logic.

**Contents**:
- **Components**: Carousel component and other generic UI elements

**Purpose**: Contains presentation components that can be used across different features.

### Feature Modules

#### Hiking Stones Module (`/hiking-stones`)
Domain-specific functionality for the Wandersteine (hiking stones) feature.

**Contents**:
- **Services**: Wanderstein service for API communication
- **Pages**: Page components used in routing (e.g., WandersteinOverviewPage)

**Purpose**: Encapsulates all business logic related to hiking stones.

## Naming Conventions

### Page Components
Components that are directly used in route definitions:
- **Suffix**: End with `Page` (e.g., `WandersteinOverviewPage`)
- **Location**: Placed in the `pages/` subdirectory of the feature module
- **Purpose**: Serve as route targets in the application

### Regular Components
Reusable components that are not route targets:
- **Suffix**: End with `Component` (e.g., `CarouselComponent`)
- **Location**: Placed in the `components/` directory
- **Purpose**: Building blocks used within pages and other components

## Barrel Exports

Each module and submodule provides a barrel export via `index.ts`:

```typescript
// Example: modules/core/index.ts
export * from './services';
export * from './components';
export * from './initializers';
```

This allows clean imports:
```typescript
import { LanguageService, TelemetryService, LanguageSwitcherComponent } from './modules/core';
```

## Adding New Modules

When creating a new feature module:

1. Create the module directory under `/modules`
2. Organize by functionality: `/services`, `/pages`, `/components`
3. Create `index.ts` barrel exports at each level
4. Follow the naming conventions for pages vs. components
5. Update tests to reflect the new structure
