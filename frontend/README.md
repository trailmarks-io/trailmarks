# TrailmarksFrontend

This project was generated using [Angular CLI](https://github.com/angular/angular-cli) version 20.1.4.

## Development server

To start a local development server, run:

```bash
ng serve
```

Once the server is running, open your browser and navigate to `http://localhost:4200/`. The application will automatically reload whenever you modify any of the source files.

## Code scaffolding

Angular CLI includes powerful code scaffolding tools. To generate a new component, run:

```bash
ng generate component component-name
```

For a complete list of available schematics (such as `components`, `directives`, or `pipes`), run:

```bash
ng generate --help
```

## Building

To build the project run:

```bash
ng build
```

This will compile your project and store the build artifacts in the `dist/` directory. By default, the production build optimizes your application for performance and speed.

## Running unit tests

The frontend has comprehensive Jasmine/Karma tests covering all components, services, and pipes.

To execute unit tests with the [Karma](https://karma-runner.github.io) test runner, use the following command:

```bash
ng test
```

To run tests in headless mode (e.g., for CI/CD):

```bash
ng test --watch=false --browsers=ChromeHeadless
```

Test coverage includes:
- Component tests (20 tests) - App, LanguageSwitcher, WandersteinOverview
- Service tests (14 tests) - WandersteinService, LanguageService
- Pipe tests (7 tests) - TranslatePipe

## Running end-to-end tests

This project uses [Playwright](https://playwright.dev/) for end-to-end testing. The E2E tests cover complete user flows and interactions, ensuring that all components work together correctly.

### E2E Test Coverage

The E2E test suite includes:
- **Homepage Tests** (4 tests) - Page loading, responsive layout, basic structure
- **Language Switcher Tests** (3 tests) - Language selection and switching functionality
- **Wanderstein Overview Tests** (6 tests) - Data loading, error handling, API mocking, empty states
- **Carousel Tests** (6 tests) - Navigation, item display, hover effects, image loading

### Running E2E Tests

To execute all E2E tests in headless mode:

```bash
npm run e2e
```

To run tests with the Playwright UI (interactive mode):

```bash
npm run e2e:ui
```

To run tests in headed mode (visible browser):

```bash
npm run e2e:headed
```

To debug tests step-by-step:

```bash
npm run e2e:debug
```

**Note**: The E2E tests automatically start the development server on port 4200. You don't need to start the server manually.

## Additional Resources

For more information on using the Angular CLI, including detailed command references, visit the [Angular CLI Overview and Command Reference](https://angular.dev/tools/cli) page.
