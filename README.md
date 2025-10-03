# Trailmarks - Wandersteine Übersicht

Eine Webanwendung zur Anzeige der zuletzt hinzugefügten Wandersteine.

## Architektur

### Backend
- **Framework**: C# ASP.NET Core 8.0
- **Datenbank**: PostgreSQL mit Entity Framework Core (SQLite Fallback für Entwicklung)
- **API Dokumentation**: OpenAPI (Swagger)
- **Features**: 
  - REST API für Wandersteine
  - Automatische Datenbankmigrationen
  - Beispieldaten für Entwicklung
  - CORS-Unterstützung
  - SQLite Fallback für lokale Entwicklung

### Frontend
- **Framework**: Angular 20.1.0
- **Styling**: CSS Grid Layout mit responsivem Design
- **HTTP Client**: Angular HttpClient für API-Kommunikation
- **Features**:
  - Übersichtsseite der 5 neuesten Wandersteine
  - Responsive Design für mobile Geräte
  - Fehlerbehandlung und Ladestatus

## API Endpunkte

- `GET /api/wandersteine/recent` - Die 5 zuletzt hinzugefügten Wandersteine
- `GET /api/wandersteine` - Alle Wandersteine
- `GET /health` - Health Check
- `GET /swagger` - API Dokumentation (interaktive Swagger UI)

## Installation und Start

### Voraussetzungen
- .NET 8.0 SDK
- Node.js 20+
- PostgreSQL (optional, SQLite wird für Entwicklung automatisch verwendet)

### Backend
```bash
cd backend
dotnet run
```

Der Backend-Server läuft auf Port 8080. Beim ersten Start wird automatisch eine SQLite-Datenbank erstellt und mit Beispieldaten gefüllt.

Für die Initialisierung der Datenbank mit Beispieldaten:
```bash
cd backend
dotnet run -- -DbInit
```

### Frontend
```bash
cd frontend
npm install
npx ng serve
```

Der Frontend-Server läuft auf Port 4200. Alternativ kann auch `npm start` verwendet werden.

## Konfiguration

Das Backend kann über `appsettings.json` oder `appsettings.Development.json` konfiguriert werden:

### Entwicklung (SQLite)
Standardmäßig wird SQLite für die lokale Entwicklung verwendet:
```json
{
  "UseSqlite": true
}
```

### Produktion (PostgreSQL)
Für die Verwendung von PostgreSQL:
```json
{
  "UseSqlite": false,
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=trailmarks;Username=postgres;Password=yourpassword"
  }
}
```

## Datenmodell

### Wanderstein Entity
Das vollständige Datenmodell in der Datenbank:
- `Id` (uint) - Primärschlüssel
- `Name` (string, max 200) - Name des Wandersteins
- `UniqueId` (string, max 50) - Eindeutige Kennung (z.B. WS-2024-001)
- `PreviewUrl` (string, max 500) - URL zum Vorschaubild
- `Description` (string, max 1000) - Beschreibungstext
- `Location` (string, max 200) - Standortinformationen
- `CreatedAt` (DateTime) - Erstellungszeitpunkt
- `UpdatedAt` (DateTime) - Letzter Änderungszeitpunkt

### API Response Format
Die API-Endpunkte geben eine vereinfachte Version zurück:
```json
{
  "id": 1,
  "name": "Schwarzwaldstein",
  "unique_id": "WS-2024-001",
  "preview_url": "https://picsum.photos/300/200?random=1",
  "created_at": "2025-08-04T12:00:00Z"
}
```

## Features

✅ REST API mit C# ASP.NET Core 8.0  
✅ PostgreSQL Datenbankintegration mit Entity Framework Core  
✅ SQLite Fallback für lokale Entwicklung  
✅ OpenAPI/Swagger Dokumentation  
✅ Angular 20.1.0 Frontend  
✅ Responsive Design  
✅ Automatische Datenbankmigrationen  
✅ Beispieldaten für Entwicklung  
✅ CORS-Unterstützung  
✅ Umfassende Fehlerbehandlung und Logging  

## Entwicklung

### Backend Tests
```bash
cd backend
dotnet test
```

### Backend Build
```bash
cd backend
dotnet build
```

### Frontend Tests
```bash
cd frontend
npx ng test
```

### Frontend Build
```bash
cd frontend
npx ng build
```

Alternativ kann Angular CLI global installiert werden:
```bash
npm install -g @angular/cli
ng build
```

### API Dokumentation
Die interaktive API-Dokumentation ist verfügbar unter: http://localhost:8080/swagger
