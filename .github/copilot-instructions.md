# GitHub Copilot Agent Instructions

Diese Datei enthält Richtlinien und Vorgaben für die Entwicklung des Trailmarks-Projekts mit GitHub Copilot.

## Architektur Dokumentation

Die Architektur Dokumentation für trailmarks ist unter https://trailmarks-io.github.io/docs/architecture einsehbar.

## Architektur Entscheidungen

Wenn Entscheidungen hinsichtlich der Architektur notwendig sind, erstelle bitte ein Issue im [trailmarks-io/docs Repository](https://github.com/trailmarks-io/docs) auf GitHub.

## Generelle Architekturvorgaben

### Repository Struktur
- **Monorepo**: Das Projekt verwendet eine Monorepo-Struktur mit separaten Verzeichnissen für Frontend und Backend
- **Strikte Trennung**: Frontend und Backend sind strikt getrennt und haben keine direkten Abhängigkeiten zueinander
- **Kommunikation**: Die Kommunikation zwischen Backend und Frontend erfolgt ausschließlich über RESTful APIs nach dem OpenAPI 3 Standard

### Verzeichnisstruktur
```
/backend    - C# ASP.NET Core Backend
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
- **Styling**: CSS (Responsive Design)
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

### Code-Standards

#### Backend (C#)
- Verwende C# Naming Conventions (PascalCase für Public Members, camelCase für lokale Variablen)
- Nutze XML-Dokumentationskommentare für öffentliche APIs
- Implementiere umfassende Fehlerbehandlung
- Verwende async/await für asynchrone Operationen
- Folge dem Repository-Pattern für Datenzugriff

#### Frontend (Angular/TypeScript)
- Verwende Angular Style Guide Konventionen
- Implementiere Reactive Forms für Formulare
- Nutze RxJS Observables für asynchrone Operationen
- Implementiere OnPush Change Detection wo möglich
- Trenne Präsentations- und Container-Komponenten

### API-Entwicklung

- Alle API-Endpunkte müssen dem OpenAPI 3 Standard entsprechen
- APIs müssen vollständig dokumentiert sein (Swagger/OpenAPI)
- Verwende RESTful Prinzipien (GET, POST, PUT, DELETE)
- Implementiere konsistente Fehlerbehandlung und HTTP-Statuscodes
- Nutze DTOs (Data Transfer Objects) für API-Responses

### Testing

#### Backend
```bash
cd backend
dotnet test
```

#### Frontend
```bash
cd frontend
npx ng test
```

### Build-Prozesse

#### Backend Build
```bash
cd backend
dotnet build
```

#### Frontend Build
```bash
cd frontend
npx ng build
```

## Akzeptanzkriterien für Issue #3

Für die Implementierung der Wandersteine-Übersichtsseite gelten folgende Akzeptanzkriterien:

- Die Daten werden mittels C# ASP.NET Core aus der Datenbank PostgreSQL geladen
- Das Backend bereitet die Daten bei API Endpunkt auf
- Der API Endpunkt unterstützt das OpenAPI Format
- Die Oberfläche wird mit Angular gebaut

## Wichtige Befehle

### Datenbank Initialisierung
```bash
cd backend
dotnet run -- -DbInit
```

### Backend starten
```bash
cd backend
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