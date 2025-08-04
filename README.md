# Trailmarks - Wandersteine Übersicht

Eine Webanwendung zur Anzeige der zuletzt hinzugefügten Wandersteine.

## Architektur

### Backend
- **Framework**: Go mit Echo Web Framework
- **Datenbank**: PostgreSQL mit GORM ORM
- **API Dokumentation**: OpenAPI (Swagger)
- **Features**: 
  - REST API für Wandersteine
  - Automatische Datenbankmigrationen
  - Beispieldaten für Entwicklung

### Frontend
- **Framework**: Angular 19
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
- `GET /swagger/*` - API Dokumentation

## Installation und Start

### Voraussetzungen
- Go 1.24+
- Node.js 20+
- PostgreSQL (optional, nutzt Standardwerte wenn nicht konfiguriert)

### Backend
```bash
cd backend
go mod download
go run main.go
```

Der Backend-Server läuft auf Port 8080.

### Frontend
```bash
cd frontend
npm install
npm start
```

Der Frontend-Server läuft auf Port 4200.

## Umgebungsvariablen

Das Backend kann über folgende Umgebungsvariablen konfiguriert werden:

- `DB_HOST` (default: localhost)
- `DB_USER` (default: postgres)
- `DB_PASSWORD` (default: postgres)
- `DB_NAME` (default: trailmarks)
- `DB_PORT` (default: 5432)
- `DB_SSLMODE` (default: disable)

## Datenmodell

### Wanderstein
```json
{
  "id": 1,
  "name": "Schwarzwaldstein",
  "unique_id": "WS-2024-001",
  "preview_url": "https://example.com/image.jpg",
  "created_at": "2024-01-15T10:30:00Z"
}
```

## Features

✅ REST API mit Echo Framework  
✅ PostgreSQL Datenbankintegration  
✅ OpenAPI Dokumentation  
✅ Angular Frontend  
✅ Responsive Design  
✅ Automatische Datenbankmigrationen  
✅ Beispieldaten für Entwicklung  
✅ CORS-Unterstützung  
✅ Fehlerbehandlung  

## Entwicklung

### Backend Tests
```bash
cd backend
go test ./...
```

### Frontend Tests
```bash
cd frontend
npm test
```

### API Dokumentation
Die interaktive API-Dokumentation ist verfügbar unter: http://localhost:8080/swagger/
