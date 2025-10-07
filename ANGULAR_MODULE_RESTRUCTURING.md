# Angular Module Restructuring

This document describes the module restructuring implemented in the Trailmarks Angular frontend application.

## Overview

The Angular frontend has been reorganized from a flat component structure into a modular architecture that separates concerns by infrastructure, shared UI components, and domain-specific features.

## Module Structure

```
frontend/src/app/modules/
├── core/                    # Infrastructure and cross-cutting concerns
│   ├── services/           # Language, Telemetry, Translation
│   ├── components/         # LanguageSwitcher
│   ├── initializers/       # App initialization logic
│   └── index.ts           # Barrel export
├── shared/                 # Reusable UI components
│   ├── components/        # Carousel and other generic UI
│   └── index.ts          # Barrel export
└── hiking-stones/         # Domain feature module
    ├── services/         # WandersteinService
    ├── pages/           # Route-targeted page components
    └── index.ts        # Barrel export
```

## Key Changes

### 1. Module Organization

**Before:**
```
src/app/
├── components/
│   ├── carousel/
│   ├── language-switcher/
│   └── wanderstein-overview/
├── services/
│   ├── language.ts
│   ├── telemetry.service.ts
│   └── wanderstein.ts
└── pipes/
    └── translate.pipe.ts
```

**After:**
```
src/app/modules/
├── core/
│   ├── services/
│   ├── components/
│   └── initializers/
├── shared/
│   └── components/
└── hiking-stones/
    ├── services/
    └── pages/
```

### 2. Naming Conventions

#### Page Components
Components used directly in routing now follow the "Page" suffix convention:

- **Old:** `WandersteinOverviewComponent`
- **New:** `WandersteinOverviewPage`

Page components:
- End with `Page` suffix
- Located in `modules/{feature}/pages/` directories
- Serve as route targets

#### Regular Components
Reusable components retain the "Component" suffix:
- Example: `CarouselComponent`, `LanguageSwitcherComponent`
- Located in `modules/{module}/components/` directories

### 3. Barrel Exports

Each module provides clean imports via `index.ts` files:

```typescript
// Before
import { LanguageService } from './services/language';
import { TelemetryService } from './services/telemetry.service';
import { TranslatePipe } from './pipes/translate.pipe';

// After
import { LanguageService, TelemetryService, TranslatePipe } from './modules/core';
```

### 4. App Initializers

App initialization logic has been extracted to the core module:

**Before:** Initializers defined in `app.config.ts`

**After:** Initializers in `modules/core/initializers/app-initializers.ts`

```typescript
export function initializeApp(languageService: LanguageService) { ... }
export function initializeTelemetry(telemetryService: TelemetryService) { ... }
```

## Module Categories

### Core Module
**Purpose:** Infrastructure services and cross-cutting concerns

**Contents:**
- `services/language.ts` - Language and translation management
- `services/telemetry.service.ts` - OpenTelemetry integration
- `services/translate.pipe.ts` - Translation pipe
- `components/language-switcher` - Language selection component
- `initializers/app-initializers.ts` - Application initialization

### Shared Module
**Purpose:** Reusable UI components without domain logic

**Contents:**
- `components/carousel` - Generic carousel component
- Future: Additional UI components (buttons, modals, etc.)

### Hiking Stones Module
**Purpose:** Domain-specific functionality for Wandersteine

**Contents:**
- `services/wanderstein.ts` - API service for hiking stones
- `pages/wanderstein-overview` - Overview page component

## Migration Benefits

1. **Better Organization:** Clear separation of concerns
2. **Improved Maintainability:** Easier to locate and modify code
3. **Scalability:** Simple to add new features and modules
4. **Testability:** Isolated modules are easier to test
5. **Clean Imports:** Barrel exports provide cleaner import statements

## Testing

All 58 tests pass successfully after the restructuring:
- Component tests
- Service tests
- Pipe tests
- Integration tests

## Build Verification

Production build succeeds with no errors:
```bash
cd frontend
npx ng build --configuration production
```

## Screenshots

### Frontend with New Structure
![Frontend Screenshot](https://github.com/user-attachments/assets/ebabdae5-3687-4117-917f-316b3a40d537)

The application loads and functions correctly with the new module structure.

## Documentation Updates

- Updated `.github/copilot-instructions.md` with module structure guidelines
- Added `frontend/src/app/modules/README.md` with detailed module documentation
- Created this summary document

## Future Additions

The modular structure supports easy addition of new features:
- New feature modules can be added under `modules/`
- Additional shared components can be added to `shared/components/`
- Core services can be extended in `core/services/`

## References

- Issue: "Angular Module Struktur im frontend"
- Commits:
  - `feat(frontend): restructure Angular app into feature modules`
  - `docs: update Copilot instructions with Angular module structure conventions`
