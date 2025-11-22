# .NET Aspire Quick Reference Guide

## Für das Trailmarks Team

Dieser Guide ist eine Kurzreferenz für die wichtigsten Aspire-Befehle und Konzepte nach der Migration.

## Quick Start (nach Migration)

### Projekt starten

```bash
# Komplette Anwendung starten
dotnet run --project aspire/Trailmarks.AppHost

# Dashboard öffnet automatisch auf: http://localhost:18888
```

**Das war's!** Alle Services (Backend, Frontend, PostgreSQL, Keycloak) starten automatisch.

### URLs

| Service | URL | Beschreibung |
|---------|-----|--------------|
| **Aspire Dashboard** | http://localhost:18888 | Logs, Traces, Metrics |
| Backend API | http://localhost:8080 | REST API |
| Swagger UI | http://localhost:8080/swagger | API Dokumentation |
| Frontend | http://localhost:4200 | Angular App |
| Keycloak | http://localhost:8180 | Authentication |

## Aspire Dashboard

### Tabs im Dashboard

| Tab | Funktion |
|-----|----------|
| **Resources** | Status aller Services, Health Checks |
| **Console** | Konsolen-Output aller Services |
| **Logs** | Strukturierte Logs mit Filtering |
| **Traces** | Distributed Tracing (ersetzt Jaeger) |
| **Metrics** | Performance-Metriken |

### Logs filtern

```
Im "Logs" Tab:
- Nach Service filtern: Dropdown oben
- Nach Level filtern: Error, Warning, Info, Debug
- Text suchen: Search Box
- Zeitbereich: Time Range Selector
```

### Traces ansehen

```
Im "Traces" Tab:
- Service auswählen: z.B. "TrailmarksApi"
- Operations filtern: z.B. "GET /api/wandersteine"
- Trace anklicken → Detaillierte Timing-Ansicht
```

## Development Workflows

### Debugging

**Visual Studio / VS Code:**
```
1. Öffne aspire/Trailmarks.AppHost/Trailmarks.AppHost.csproj
2. Drücke F5
3. Setze Breakpoints in TrailmarksApi
4. Breakpoints funktionieren sofort
```

**Einzelnes Projekt debuggen:**
```bash
# Nur Backend starten
cd backend/src/TrailmarksApi
dotnet run

# Nur Frontend starten
cd frontend
npm start
```

### Code-Änderungen

**Backend-Code geändert:**
```bash
# Aspire neu starten
Ctrl+C
dotnet run --project aspire/Trailmarks.AppHost

# Oder: Hot Reload in IDE (wenn unterstützt)
```

**Frontend-Code geändert:**
```
# Automatisches Hot Reload (npm start)
# Keine Aktion nötig
```

**AppHost-Code geändert:**
```bash
# Neu starten für neue Konfiguration
Ctrl+C
dotnet run --project aspire/Trailmarks.AppHost
```

## Wichtige Aspire-Konzepte

### Service Discovery

Services finden sich automatisch:

```csharp
// Im AppHost
var backend = builder.AddProject<TrailmarksApi>("backend");
var frontend = builder.AddNpmApp("frontend")
    .WithReference(backend);  // Frontend kennt Backend-URL automatisch
```

Im Frontend (wird automatisch injiziert):
```typescript
// environment.ts hat automatisch die richtige Backend-URL
```

### Connection Strings

Connection Strings werden automatisch injiziert:

```csharp
// Im AppHost
var postgres = builder.AddPostgres("postgres");
var trailmarksDb = postgres.AddDatabase("trailmarks");

var backend = builder.AddProject<TrailmarksApi>("backend")
    .WithReference(trailmarksDb);  // Connection String automatisch!
```

Im Backend (TrailmarksApi):
```csharp
// Program.cs
var connectionString = builder.Configuration.GetConnectionString("trailmarks");
// → Hat automatisch den richtigen Wert von Aspire
```

### Health Checks

Aspire zeigt automatisch Health Status:

```
Dashboard → "Resources" Tab:
- Grün: Service läuft und ist healthy
- Gelb: Service startet noch
- Rot: Service failed
```

### OpenTelemetry

OpenTelemetry ist automatisch konfiguriert via ServiceDefaults:

```csharp
// backend/src/TrailmarksApi/Program.cs
var builder = WebApplication.CreateBuilder(args);

// Diese Zeile aktiviert alles:
builder.AddServiceDefaults();

// Traces, Logs, Metrics gehen automatisch zum Aspire Dashboard
```

## Häufige Aufgaben

### Services neustarten

```bash
# Alles neu starten
Ctrl+C
dotnet run --project aspire/Trailmarks.AppHost
```

### Datenbank zurücksetzen

```bash
# Container und Volumes löschen
docker ps -a | grep postgres | awk '{print $1}' | xargs docker rm -f
docker volume prune -f

# Aspire neu starten (erstellt DB neu)
dotnet run --project aspire/Trailmarks.AppHost
```

### Logs anschauen

**Option 1: Aspire Dashboard**
```
http://localhost:18888 → "Logs" Tab
```

**Option 2: Console**
```
http://localhost:18888 → "Console" Tab
→ Zeigt Raw Console Output
```

**Option 3: CLI**
```bash
# Aspire läuft im Terminal, Ctrl+C stoppt
# Logs werden direkt im Terminal angezeigt
```

### Performance-Probleme debuggen

```
1. Aspire Dashboard → "Traces" Tab
2. Service "TrailmarksApi" auswählen
3. Langsame Operations finden (sortiere nach Duration)
4. Trace anklicken → Siehst Timing-Breakdown
5. Finde langsame DB-Queries oder HTTP-Calls
```

## Projekt-Struktur (nach Migration)

```
trailmarks/
├── aspire/
│   └── Trailmarks.AppHost/          # Orchestration
│       ├── Program.cs                # Service-Konfiguration
│       └── Trailmarks.AppHost.csproj
├── backend/
│   └── src/
│       ├── TrailmarksApi/            # Backend API (wie vorher)
│       └── TrailmarksApi.ServiceDefaults/  # Shared Config (NEU)
│           ├── Extensions.cs         # AddServiceDefaults()
│           └── TrailmarksApi.ServiceDefaults.csproj
├── frontend/                         # Angular (unverändert)
└── trailmarks.sln                    # Solution (updated)
```

## Vergleich: Vorher vs. Nachher

### Projekt starten

| Vorher (Docker Compose) | Nachher (Aspire) |
|-------------------------|------------------|
| `docker-compose up -d` | `dotnet run --project aspire/Trailmarks.AppHost` |
| Warten bis Services ready | Startet automatisch koordiniert |
| Logs: `docker-compose logs -f backend` | Dashboard: http://localhost:18888 |
| Traces: http://localhost:16686 (Jaeger) | Dashboard: http://localhost:18888 |
| Services stoppen: `docker-compose down` | Ctrl+C |

### Debugging

| Vorher (Docker Compose) | Nachher (Aspire) |
|-------------------------|------------------|
| Remote Debugging Setup | F5 in IDE |
| Container Logs folgen | Breakpoints direkt |
| Trace in Jaeger suchen | Dashboard zeigt alles |

### Observability

| Vorher | Nachher (Aspire Dashboard) |
|--------|----------------------------|
| Jaeger UI (Traces) | ✅ Traces Tab |
| docker logs (Logs) | ✅ Logs Tab (strukturiert) |
| Health Checks manuell | ✅ Resources Tab |
| Metrics fehlen | ✅ Metrics Tab |

## Troubleshooting

### "Service failed to start"

```
1. Aspire Dashboard → "Resources" Tab
2. Service anklicken (roter Status)
3. "View logs" → Siehst Error-Messages
4. Häufige Ursachen:
   - Port schon belegt
   - Connection String falsch
   - Dependencies nicht bereit
```

### "Cannot connect to database"

```bash
# PostgreSQL Status prüfen
docker ps | grep postgres

# Wenn nicht läuft: Aspire neu starten
Ctrl+C
dotnet run --project aspire/Trailmarks.AppHost
```

### "Dashboard does not open"

```
Dashboard sollte automatisch öffnen auf http://localhost:18888

Falls nicht:
1. Manuell Browser öffnen: http://localhost:18888
2. Port-Konflikt? Aspire Config anpassen:
   - Siehe Trailmarks.AppHost/Program.cs
   - Dashboard Port änderbar via Config
```

### "Frontend can't reach Backend"

```
Frontend sollte Backend-URL automatisch haben via Service Discovery.

Prüfen:
1. Aspire Dashboard → "Resources" → "backend" Status
2. Backend sollte "Running" und Healthy sein
3. Frontend Environment sollte backend-URL haben
```

## Commands Cheat Sheet

```bash
# === Entwicklung ===

# Alles starten (Development)
dotnet run --project aspire/Trailmarks.AppHost

# Nur Backend (für Backend-only Development)
cd backend/src/TrailmarksApi && dotnet run

# Nur Frontend (für Frontend-only Development)
cd frontend && npm start

# === Testing ===

# Backend Tests
cd backend && dotnet test

# Frontend Unit Tests
cd frontend && npm test

# Frontend E2E Tests
cd frontend && npm run e2e

# === Build ===

# Backend Build
cd backend/src/TrailmarksApi && dotnet build

# Frontend Build
cd frontend && npm run build

# === Datenbank ===

# DB Migrationen erstellen
cd backend/src/TrailmarksApi && dotnet ef migrations add MigrationName

# DB initialisieren (via Backend)
cd backend/src/TrailmarksApi && dotnet run -- -DbInit

# === Cleanup ===

# Docker Containers aufräumen
docker ps -a | grep trailmarks | awk '{print $1}' | xargs docker rm -f

# Docker Volumes aufräumen
docker volume prune -f

# .NET Build-Artefakte löschen
dotnet clean
```

## Best Practices

### Development Workflow

1. **Starte mit Aspire Dashboard**
   ```bash
   dotnet run --project aspire/Trailmarks.AppHost
   ```

2. **Nutze Dashboard für Debugging**
   - Logs: Strukturierte Logs mit Filtering
   - Traces: Performance-Probleme finden
   - Console: Raw Output bei Bedarf

3. **Einzelne Services bei Bedarf**
   - Backend allein: `cd backend/src/TrailmarksApi && dotnet run`
   - Frontend allein: `cd frontend && npm start`

4. **Tests regelmäßig laufen lassen**
   ```bash
   # Backend
   cd backend && dotnet test
   
   # Frontend
   cd frontend && npm test
   ```

### Code-Änderungen

1. **Backend Code geändert**
   - Hot Reload funktioniert in IDE
   - Oder: Aspire neu starten

2. **Frontend Code geändert**
   - Hot Reload funktioniert automatisch (npm)

3. **Service-Konfiguration geändert**
   - AppHost neu starten (Ctrl+C + dotnet run)

4. **Dependencies geändert**
   - NuGet Restore: Automatisch bei Build
   - npm install: `cd frontend && npm install`

## Weitere Ressourcen

- **Aspire Dashboard**: http://localhost:18888 (automatisch)
- **Detaillierter Plan**: [ASPIRE_MIGRATION_PLAN.md](ASPIRE_MIGRATION_PLAN.md)
- **Vergleich**: [ASPIRE_VS_DOCKER_COMPOSE.md](ASPIRE_VS_DOCKER_COMPOSE.md)
- **Microsoft Docs**: https://learn.microsoft.com/en-us/dotnet/aspire/

## Fragen?

Bei Problemen oder Fragen zur Aspire-Migration:
1. Siehe [ASPIRE_MIGRATION_PLAN.md](ASPIRE_MIGRATION_PLAN.md)
2. Aspire Dashboard → Logs/Traces für Debugging
3. Team Slack Channel
4. GitHub Issues

---

**Happy Coding mit .NET Aspire! 🚀**
