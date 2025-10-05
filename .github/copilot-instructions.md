# GitHub Copilot Agent Instructions

Diese Datei enthält Richtlinien und Vorgaben für die Entwicklung des Trailmarks-Projekts mit GitHub Copilot.

## Generelle Architekturvorgaben

### Repository Struktur
- **Monorepo**: Das Projekt verwendet eine Monorepo-Struktur mit separaten Verzeichnissen für Frontend und Backend
- **Strikte Trennung**: Frontend und Backend sind strikt getrennt und haben keine direkten Abhängigkeiten zueinander
- **Kommunikation**: Die Kommunikation zwischen Backend und Frontend erfolgt ausschließlich über RESTful APIs nach dem OpenAPI 3 Standard

### Verzeichnisstruktur
```
/backend
  /src      - C# ASP.NET Core Backend (Produktivcode)
  /test     - xUnit Tests für Backend
/frontend   - Angular Frontend
```

## Technologievorgaben

### Backend
- **Framework**: .NET 8.0 (neueste stabile Version)
- **Sprache**: C# mit ASP.NET Core
- **ORM**: Entity Framework Core
- **API-Dokumentation**: OpenAPI 3 / Swagger
- **Datenbank**: PostgreSQL (Primär), SQLite (Entwicklung/Fallback)
- **Features**: 
  - RESTful API Endpoints
  - Automatische Datenbankmigrationen
  - CORS-Unterstützung
  - Umfassende Fehlerbehandlung und Logging

### Frontend
- **Framework**: Angular 20.1.0 (neueste stabile Version)
- **Sprache**: TypeScript
- **Styling**: Tailwind CSS v3 (Utility-First CSS Framework)
- **HTTP Client**: Angular HttpClient für API-Kommunikation
- **Features**:
  - Responsive Design für mobile Geräte
  - Fehlerbehandlung und Ladestatus
  - Single Page Application (SPA)

### Persistenz
- **Primäre Datenbank**: PostgreSQL
- **Entwicklungsdatenbank**: SQLite als Fallback
- **ORM**: Entity Framework Core mit Code-First-Ansatz
- **Migrations**: Automatische Datenbankmigrationen

## Entwicklungsrichtlinien

### Grundlegende Anweisungen

1. **Bei unklaren Aufgaben**:
   - Triff keine Annahmen
   - Frage explizit nach, wenn Anforderungen unklar sind
   - Dokumentiere Entscheidungen, die getroffen werden müssen

2. **Technologie und Abhängigkeiten**:
   - Führe keine neuen Technologien ohne ausdrückliche Erlaubnis ein
   - Füge keine neuen Abhängigkeiten (NuGet-Pakete, npm-Pakete) ohne Genehmigung hinzu
   - Verwende die bereits im Projekt vorhandenen Bibliotheken und Frameworks

3. **Entwicklungsprozess**:
   - Trenne Feature-Entwicklung klar von Refactorings
   - Halte Pull Requests klein und fokussiert
   - Ein PR sollte entweder neue Features ODER Refactoring enthalten, nicht beides
   - Dokumentiere größere Änderungen im Code mit Kommentaren

4. **Design-Prinzipien**:
   - **Composition over Inheritance**: Bevorzuge Komposition gegenüber Vererbung
   - Verwende statische Helper-Klassen oder Dependency Injection statt Vererbungshierarchien
   - Halte Klassen fokussiert und folge dem Single Responsibility Principle
   - Vermeide tiefe Vererbungshierarchien, die schwer zu testen und zu warten sind

5. **Testing**:
   - Schreibe Unit Tests für alle neuen Komponenten, Services und API-Endpunkte
   - Tests müssen vor dem Mergen eines PRs alle erfolgreich durchlaufen
   - Backend: Verwende xUnit mit Moq und In-Memory Database
   - Frontend: Verwende Jasmine/Karma mit TestBed und Spies
   - Teste sowohl Success- als auch Error-Szenarien
   - Halte Tests einfach, lesbar und wartbar

### Git Branching Strategie

Das Projekt verwendet **Git Flow** als Branching-Strategie:

1. **Main Branch**:
   - `main` ist der Master Branch
   - Enthält nur stabilen, produktionsreifen Code
   - Direkte Commits auf `main` sind nicht erlaubt

2. **Develop Branch**:
   - `develop` ist der primäre Entwicklungsbranch
   - Alle Feature Branches werden von `develop` abgezweigt
   - Feature Branches werden nach Fertigstellung in `develop` gemergt
   - Direkte Commits auf `develop` sollten vermieden werden

3. **Feature Branches**:
   - Feature Branches werden für neue Features oder Bugfixes erstellt
   - Naming Convention: `<issue-nummer>-kurze-beschreibung`
   - Beispiel: `#42-add-user-authentication` (für GitHub Issue #42)
   - Feature Branches werden immer von `develop` abgezweigt
   - Nach Fertigstellung werden Feature Branches via Pull Request in `develop` gemergt

4. **Commit Messages**:
   - Alle Commits folgen dem **Conventional Commits** Standard
   - Format: `<type>(<scope>): <subject>`
   - Types: 
     - `feat`: Neue Features
     - `fix`: Bugfixes
     - `docs`: Dokumentationsänderungen
     - `style`: Code-Formatierung (keine funktionalen Änderungen)
     - `refactor`: Code-Refactoring
     - `test`: Tests hinzufügen oder ändern
     - `chore`: Build-Prozess oder Tool-Änderungen
   - Beispiele:
     - `feat(backend): add user authentication endpoint`
     - `fix(frontend): resolve navigation issue`
     - `docs: update installation instructions`

5. **Pull Request Workflow**:
   - Erstelle einen Feature Branch basierend auf der Issue-Nummer
   - Entwickle das Feature mit kleinen, atomaren Commits
   - Pushe regelmäßig zum Remote Repository
   - Erstelle einen Pull Request zum Mergen in `develop`
   - Nach Code Review und erfolgreichen Tests wird der PR gemergt

### Code-Standards

#### Backend (C#)
- Verwende C# Naming Conventions (PascalCase für Public Members, camelCase für lokale Variablen)
- Nutze XML-Dokumentationskommentare für öffentliche APIs
- Implementiere umfassende Fehlerbehandlung
- Verwende async/await für asynchrone Operationen
- Folge dem Repository-Pattern für Datenzugriff
- **Composition over Inheritance**: Bevorzuge Komposition gegenüber Vererbung, um lose Kopplung und bessere Testbarkeit zu erreichen

#### Frontend (Angular/TypeScript)
- Verwende Angular Style Guide Konventionen
- Implementiere Reactive Forms für Formulare
- Nutze RxJS Observables für asynchrone Operationen
- Implementiere OnPush Change Detection wo möglich
- Trenne Präsentations- und Container-Komponenten

#### Styling mit Tailwind CSS
- **Ausschließliche Verwendung**: Nutze für alle Styles und Layouts ausschließlich Tailwind CSS Utility-Klassen
- **Keine separaten CSS-Dateien**: Erstelle keine komponentenspezifischen CSS-Dateien (*.css), sondern verwende Tailwind-Klassen direkt in den HTML-Templates
- **Responsive Design**: Verwende Tailwinds responsive Breakpoints (sm:, md:, lg:, xl:, 2xl:) für mobile-first Design
- **Utility-First-Ansatz**: Bevorzuge Utility-Klassen gegenüber benutzerdefinierten CSS-Klassen
- **Konsistenz**: Nutze Tailwinds Designsystem für Farben, Abstände, Schriftgrößen etc., um ein konsistentes Design zu gewährleisten
- **Hover/Focus States**: Nutze Tailwinds State-Varianten (hover:, focus:, active:) für interaktive Elemente
- **Transitions**: Verwende Tailwinds Transition-Utilities (transition, duration, ease) für sanfte Animationen
- **Custom Styles**: Falls unbedingt erforderlich, definiere custom Styles in der globalen styles.css mit @layer-Direktiven

### API-Entwicklung

- Alle API-Endpunkte müssen dem OpenAPI 3 Standard entsprechen
- APIs müssen vollständig dokumentiert sein (Swagger/OpenAPI)
- Verwende RESTful Prinzipien (GET, POST, PUT, DELETE)
- Implementiere konsistente Fehlerbehandlung und HTTP-Statuscodes
- Nutze DTOs (Data Transfer Objects) für API-Responses
- **Fehlerbehandlung**: Verwende den ProblemDetails Standard (RFC 7807) für alle Fehlerantworten
  - Nutze die `Problem()` Methode von `ControllerBase` für strukturierte Fehlerantworten
  - ProblemDetails enthält: `type`, `title`, `status`, `detail` und optionale `instance`
  - Beispiel: `return Problem(title: "Resource not found", statusCode: 404, detail: "The requested Wanderstein was not found")`
  - Für 500er Fehler: Verwende `Problem()` ohne Details, um keine internen Informationen preiszugeben

### Testing

Das Projekt verfolgt einen testgetriebenen Ansatz mit umfassenden Unit Tests für alle Komponenten.

#### Backend (xUnit)

- **Framework**: xUnit als primäres Test-Framework
- **Test-Projekt**: `backend/test/` (innerhalb des backend Verzeichnisses)
- **Produktivcode**: `backend/src/` (enthält Controller, Services, Models, etc.)
- **Struktur**: Tests spiegeln die Struktur des Hauptprojekts wider
  - `Controllers/` - Controller-Tests
  - `Services/` - Service-Tests
  - `Models/` - Model-Tests
- **Dependencies**: 
  - xUnit für Test-Framework
  - Moq für Mocking
  - Microsoft.EntityFrameworkCore.InMemory für Datenbanktest
  - Microsoft.AspNetCore.Mvc.Testing für Integration Tests

**Test-Richtlinien**:
- Jeder Controller, Service und jede Model-Klasse benötigt Unit Tests
- Verwende In-Memory Datenbank für Datenbank-abhängige Tests
- Teste alle öffentlichen Methoden und API-Endpunkte
- Teste Edge Cases und Fehlerbehandlung
- Verwende Arrange-Act-Assert Pattern

**Tests ausführen**:
```bash
cd backend/test
dotnet test
```

#### Frontend (Jasmine/Karma)

- **Framework**: Jasmine mit Karma Test Runner
- **Test-Dateien**: `*.spec.ts` Dateien neben den zu testenden Dateien
- **Struktur**: 
  - Component Tests
  - Service Tests
  - Pipe Tests

**Test-Richtlinien**:
- Jede Component, jeder Service und jede Pipe benötigt Unit Tests
- Verwende TestBed für Angular Dependency Injection
- Verwende HttpTestingController für HTTP-Mock
- Teste alle öffentlichen Methoden und Event Handler
- Teste User Interactions und Lifecycle Hooks
- Verwende Jasmine Spies für Mocking

**Tests ausführen**:
```bash
cd frontend
npx ng test
```

**Tests im Headless Mode ausführen**:
```bash
cd frontend
npx ng test --watch=false --browsers=ChromeHeadless
```

### Build-Prozesse

#### Backend Build
```bash
cd backend/src
dotnet build
```

#### Frontend Build
```bash
cd frontend
npx ng build
```

## Wichtige Befehle

### Datenbank Initialisierung
```bash
cd backend/src
dotnet run -- -DbInit
```

### Backend starten
```bash
cd backend/src
dotnet run
```
Server läuft auf: http://localhost:8080
Swagger UI verfügbar unter: http://localhost:8080/swagger

### Frontend starten
```bash
cd frontend
npm install
npx ng serve
```
Server läuft auf: http://localhost:4200