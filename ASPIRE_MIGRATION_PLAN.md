# Migration von Docker Compose zu .NET Aspire

## Übersicht

Dieses Dokument beschreibt den detaillierten Plan zur Migration des Trailmarks-Projekts von Docker Compose zu .NET Aspire. Die Migration wird die Entwicklererfahrung verbessern und die Orchestrierung durch C#-basierten Code modernisieren.

## Motivation

### Warum .NET Aspire?

**Vorteile gegenüber Docker Compose:**

1. **Entwicklererfahrung**
   - Typsichere C#-Konfiguration statt YAML
   - IntelliSense und Compile-Time-Validierung
   - Debugging-Unterstützung direkt in der IDE
   - Integriertes Aspire Dashboard für Observability

2. **Vereinfachte Orchestrierung**
   - Automatische Service Discovery
   - Integrierte Konfigurationsverwaltung
   - Nahtlose OpenTelemetry-Integration
   - Health Checks out-of-the-box

3. **Modernisierung**
   - Native .NET-Integration
   - Bessere Unterstützung für Cloud-Deployment
   - Vereinfachte Dependency-Verwaltung
   - Reduzierung von YAML-Konfigurationsdateien

4. **Observability**
   - Aspire Dashboard ersetzt Jaeger UI
   - Integrierte Logs, Metrics und Traces in einer Oberfläche
   - Echtzeit-Visualisierung während der Entwicklung
   - Keine separaten NGINX-Proxies für OTLP nötig

## Aktuelle Architektur (Docker Compose)

### Services

1. **Jaeger** - OpenTelemetry Tracing Backend
   - Port: 16686 (UI), 4317 (OTLP gRPC), 4318 (OTLP HTTP)
   - Funktion: Trace-Sammlung und Visualisierung

2. **NGINX OTLP Proxy**
   - Port: 6060
   - Funktion: CORS-Proxy für Frontend → Jaeger

3. **PostgreSQL mit PostGIS**
   - Port: 5432
   - Datenbanken: `trailmarks`, `keycloak`
   - Volume: `postgres-data`

4. **Keycloak**
   - Port: 8180
   - Funktion: Authentication und Authorization
   - Realm: `trailmarks`

5. **Backend (TrailmarksApi)**
   - Port: 8080
   - Framework: .NET 9.0 ASP.NET Core
   - Features: REST API, Entity Framework Core, OpenTelemetry

6. **Frontend**
   - Port: 4200
   - Framework: Angular 20.1
   - Features: SPA, Tailwind CSS, OpenTelemetry

### Abhängigkeiten

```
Frontend → Backend → PostgreSQL
         ↓         ↓
    NGINX-OTLP → Jaeger
         ↓
    Keycloak → PostgreSQL
```

## Ziel-Architektur (.NET Aspire)

### Neue Projekt-Struktur

```
trailmarks/
├── backend/
│   ├── src/
│   │   ├── TrailmarksApi/              # Existing API
│   │   └── TrailmarksApi.ServiceDefaults/  # NEW: Aspire Service Defaults
│   └── test/
├── frontend/                           # Bleibt unverändert (Angular)
├── aspire/                            # NEW: Aspire Orchestration
│   ├── Trailmarks.AppHost/           # NEW: AppHost Projekt
│   └── Trailmarks.ServiceDefaults/    # OPTIONAL: Shared defaults (if needed)
└── docker-compose.yml                 # DEPRECATED (kann als Fallback bleiben)
```

### Aspire Components

1. **AppHost Projekt** (`aspire/Trailmarks.AppHost`)
   - Orchestriert alle Services
   - Definiert Abhängigkeiten in C#
   - Startet Aspire Dashboard

2. **Service Defaults** (`backend/src/TrailmarksApi.ServiceDefaults`)
   - Gemeinsame Konfiguration für .NET Services
   - OpenTelemetry-Setup
   - Health Checks
   - Service Discovery

3. **Aspire Dashboard**
   - Ersetzt Jaeger UI
   - Integrierte Logs, Metrics, Traces
   - Automatisch auf Port 18888 (configurable)

### Service-Mapping

| Docker Compose Service | Aspire Equivalent | Notes |
|------------------------|-------------------|-------|
| Backend | `builder.AddProject<TrailmarksApi>()` | .NET Projekt direkt |
| Frontend | Container oder externe Resource | Angular als Container oder npm dev server |
| PostgreSQL | `builder.AddPostgres().AddDatabase()` | Aspire Hosting Component |
| Keycloak | `builder.AddContainer()` | Container Component |
| Jaeger | **Entfällt** | Ersetzt durch Aspire Dashboard |
| NGINX OTLP | **Entfällt** | Nicht mehr nötig |

## Migrations-Schritte

### Phase 1: Vorbereitung (Nicht-Breaking)

**Ziel:** Aspire-Struktur parallel zu Docker Compose aufbauen

**Schritte:**

1. **Aspire Templates installieren** ✅ (bereits erledigt)
   ```bash
   dotnet new install Aspire.ProjectTemplates
   ```

2. **AppHost Projekt erstellen**
   ```bash
   cd aspire
   dotnet new aspire-apphost -n Trailmarks.AppHost
   ```

3. **Service Defaults Projekt erstellen**
   ```bash
   cd backend/src
   dotnet new aspire-servicedefaults -n TrailmarksApi.ServiceDefaults
   ```

4. **Referenzen hinzufügen**
   - TrailmarksApi → TrailmarksApi.ServiceDefaults
   - AppHost → TrailmarksApi

5. **Solution-Datei aktualisieren**
   ```bash
   dotnet sln add aspire/Trailmarks.AppHost/Trailmarks.AppHost.csproj
   dotnet sln add backend/src/TrailmarksApi.ServiceDefaults/TrailmarksApi.ServiceDefaults.csproj
   ```

**Ergebnis:** Aspire-Struktur existiert, Docker Compose funktioniert weiter

### Phase 2: Backend Migration

**Ziel:** Backend-API in Aspire integrieren

**Schritte:**

1. **TrailmarksApi anpassen**
   - `ServiceDefaults` Paket referenzieren
   - `builder.AddServiceDefaults()` in Program.cs
   - OpenTelemetry Config aus Program.cs nach ServiceDefaults verschieben

2. **AppHost konfigurieren**
   ```csharp
   var builder = DistributedApplication.CreateBuilder(args);
   
   // PostgreSQL
   var postgres = builder.AddPostgres("postgres")
       .WithPgAdmin()
       .AddDatabase("trailmarks");
   
   // Keycloak
   var keycloak = builder.AddContainer("keycloak", "quay.io/keycloak/keycloak", "26.0.7")
       .WithEnvironment("KC_DB", "postgres")
       .WithEnvironment("KEYCLOAK_ADMIN", "admin")
       .WithEnvironment("KEYCLOAK_ADMIN_PASSWORD", "admin")
       .WithHttpEndpoint(port: 8180, targetPort: 8080, name: "http");
   
   // Backend API
   var backend = builder.AddProject<TrailmarksApi>("backend")
       .WithReference(postgres)
       .WithReference(keycloak);
   
   builder.Build().Run();
   ```

3. **Connection Strings anpassen**
   - Aspire injiziert automatisch Connection Strings
   - Keycloak-Connection manuell konfigurieren

**Ergebnis:** Backend läuft mit Aspire Dashboard

### Phase 3: Frontend Integration

**Ziel:** Angular Frontend in Aspire integrieren

**Optionen:**

**Option A: Container (empfohlen für Produktion)**
```csharp
var frontend = builder.AddContainer("frontend", "node", "20-alpine")
    .WithBindMount("./frontend", "/app")
    .WithWorkingDirectory("/app")
    .WithEntrypoint("npm", "run", "start")
    .WithHttpEndpoint(port: 4200, name: "http")
    .WithReference(backend);
```

**Option B: npm Executable (für Entwicklung)**
```csharp
var frontend = builder.AddNpmApp("frontend", "../frontend")
    .WithNpmCommand("start")
    .WithHttpEndpoint(port: 4200, name: "http")
    .WithReference(backend);
```

**Schritte:**

1. Frontend-Integration ins AppHost
2. Environment-Variable für Backend-URL setzen
3. OTLP Endpoint auf Aspire Dashboard umleiten

**Ergebnis:** Vollständige Anwendung läuft in Aspire

### Phase 4: Keycloak und PostGIS

**Ziel:** Vollständige Feature-Parität mit Docker Compose

**Schritte:**

1. **PostgreSQL mit PostGIS**
   ```csharp
   var postgres = builder.AddPostgres("postgres", password: "postgres")
       .WithImage("postgis/postgis", "16-3.4-alpine")
       .WithDataVolume()
       .AddDatabase("trailmarks")
       .AddDatabase("keycloak");
   ```

2. **Keycloak Volume Mounts**
   ```csharp
   var keycloak = builder.AddContainer("keycloak", "quay.io/keycloak/keycloak", "26.0.7")
       .WithBindMount("./keycloak/realm-export.json", "/opt/keycloak/data/import/realm-export.json", isReadOnly: true)
       .WithEnvironment("KC_DB_URL", $"jdbc:postgresql://postgres:5432/keycloak")
       // ... weitere Config
       .WithHttpEndpoint(port: 8180, targetPort: 8080);
   ```

3. **Startup Reihenfolge**
   - `WaitFor()` für Dependencies nutzen

**Ergebnis:** Alle Services in Aspire mit vollständiger Funktionalität

### Phase 5: Cleanup und Dokumentation

**Ziel:** Aufräumen und Dokumentation aktualisieren

**Schritte:**

1. **Docker Compose entfernen oder deprecaten**
   - `docker-compose.yml` → `docker-compose.yml.deprecated`
   - Oder: Docker Compose als Fallback behalten mit Hinweis

2. **NGINX OTLP Config entfernen**
   - `nginx-otlp.conf` löschen
   - NGINX Service aus compose entfernen

3. **Dokumentation aktualisieren**
   - `README.md`: Aspire statt Docker Compose
   - `DOCKER.md` → `ASPIRE.md`
   - Neue Befehle dokumentieren:
     ```bash
     # Starten
     dotnet run --project aspire/Trailmarks.AppHost
     
     # Dashboard
     http://localhost:18888
     ```

4. **GitHub Workflows anpassen**
   - CI/CD auf Aspire anpassen
   - Build-Prozesse aktualisieren

**Ergebnis:** Sauberes Repository mit Aspire als Standard

## Neue Entwickler-Workflows

### Lokale Entwicklung

**Vorher (Docker Compose):**
```bash
docker-compose up -d
# Warten bis Services ready
docker-compose logs -f backend
```

**Nachher (Aspire):**
```bash
dotnet run --project aspire/Trailmarks.AppHost
# Browser öffnet automatisch Dashboard auf http://localhost:18888
```

### Debugging

**Vorher:**
- Services einzeln debuggen
- Logs über docker-compose logs
- Traces in Jaeger UI (http://localhost:16686)

**Nachher:**
- F5 in Visual Studio / VS Code
- Alle Services im Debugger
- Logs, Metrics, Traces in Aspire Dashboard (http://localhost:18888)
- Echtzeit-Visualisierung

### Testing

**Backend Tests:** Unverändert
```bash
cd backend && dotnet test
```

**Frontend Tests:** Unverändert
```bash
cd frontend && npm run test
```

**E2E Tests:** Backend-URL auf Aspire anpassen

## Aspire Dashboard Features

Das Aspire Dashboard ersetzt mehrere Tools:

1. **Jaeger UI** → Traces im Dashboard
2. **Docker Logs** → Strukturierte Logs im Dashboard
3. **Health Checks** → Status-Übersicht im Dashboard
4. **Metrics** → Performance-Metriken im Dashboard

**Features:**
- ✅ Verteilte Tracing (OpenTelemetry)
- ✅ Strukturierte Logs mit Filtering
- ✅ Metrics und Performance-Daten
- ✅ Service-Abhängigkeiten visualisiert
- ✅ Health Status aller Services
- ✅ Echtzeit-Updates
- ✅ Keine separate Installation nötig

## Risiken und Mitigationen

### Risiko 1: Frontend Integration

**Problem:** Angular ist kein .NET Projekt

**Mitigation:**
- Container-basierte Integration (wie bisher)
- Oder: npm-based Executable Component
- Fallback: Frontend separat starten

### Risiko 2: Keycloak Komplexität

**Problem:** Komplexe Keycloak-Config mit Volumes und Init-Scripts

**Mitigation:**
- Container Component mit allen Mounts
- Init-Script als Volume Mount
- Health Checks für Startup-Reihenfolge

### Risiko 3: PostGIS Extension

**Problem:** Standard Postgres Image hat kein PostGIS

**Mitigation:**
- Custom Image mit `WithImage("postgis/postgis", "16-3.4-alpine")`
- Funktioniert wie in Docker Compose

### Risiko 4: Team Adoption

**Problem:** Team muss neue Technologie lernen

**Mitigation:**
- Umfassende Dokumentation
- Docker Compose als Fallback behalten
- Training und Onboarding

## Vorteile der Migration

### Entwickler-Produktivität

- ⚡ Schnellerer Start (1 Befehl statt docker-compose)
- 🐛 Besseres Debugging (alle Services in IDE)
- 📊 Bessere Observability (Aspire Dashboard)
- 🔧 Weniger Kontext-Wechsel

### Code-Qualität

- 🎯 Typsichere Konfiguration
- ✅ Compile-Time Validierung
- 🔍 IntelliSense Support
- 🧪 Testbare Orchestrierung

### Wartbarkeit

- 📦 Alle Konfiguration in C#
- 🔄 Einfacheres Refactoring
- 📚 Bessere IDE-Integration
- 🚀 Modernere Technologie

## Timeline

| Phase | Dauer | Status |
|-------|-------|--------|
| Phase 1: Vorbereitung | 2 Tage | Geplant |
| Phase 2: Backend Migration | 3 Tage | Geplant |
| Phase 3: Frontend Integration | 2 Tage | Geplant |
| Phase 4: Keycloak & PostGIS | 2 Tage | Geplant |
| Phase 5: Cleanup & Docs | 2 Tage | Geplant |
| **Gesamt** | **~2 Wochen** | - |

## Erfolgs-Kriterien

- ✅ Alle Services laufen mit Aspire
- ✅ Aspire Dashboard zeigt Traces, Logs, Metrics
- ✅ Backend-Tests bestehen alle
- ✅ Frontend-Tests bestehen alle
- ✅ E2E-Tests bestehen alle
- ✅ Dokumentation aktualisiert
- ✅ Team kann mit Aspire arbeiten
- ✅ Performance mindestens gleich gut wie vorher

## Referenzen

- [Microsoft Learn: .NET Aspire Overview](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview)
- [Microsoft Learn: Migrate from Docker Compose to Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/migrate-from-docker-compose)
- [Aspire Dashboard Documentation](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/dashboard)
- [Aspire Telemetry](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/telemetry)
- [Aspire Components](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/components-overview)

## Anhang: Beispiel AppHost Code

```csharp
using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// PostgreSQL mit PostGIS
var postgres = builder.AddPostgres("postgres", password: "postgres", port: 5432)
    .WithImage("postgis/postgis", "16-3.4-alpine")
    .WithDataVolume()
    .WithEnvironment("TZ", "Europe/Berlin");

var trailmarksDb = postgres.AddDatabase("trailmarks");
var keycloakDb = postgres.AddDatabase("keycloak");

// Keycloak
var keycloak = builder.AddContainer("keycloak", "quay.io/keycloak/keycloak", "26.0.7")
    .WithBindMount("./keycloak/realm-export.json", "/opt/keycloak/data/import/realm-export.json", isReadOnly: true)
    .WithBindMount("./keycloak/init-keycloak-db.sh", "/docker-entrypoint-initdb.d/init-keycloak-db.sh", isReadOnly: true)
    .WithEnvironment("KC_DB", "postgres")
    .WithEnvironment("KC_DB_URL", "jdbc:postgresql://postgres:5432/keycloak")
    .WithEnvironment("KC_DB_USERNAME", "postgres")
    .WithEnvironment("KC_DB_PASSWORD", "postgres")
    .WithEnvironment("KC_HOSTNAME", "localhost")
    .WithEnvironment("KC_HOSTNAME_PORT", "8180")
    .WithEnvironment("KC_HOSTNAME_STRICT", "false")
    .WithEnvironment("KC_HOSTNAME_STRICT_HTTPS", "false")
    .WithEnvironment("KC_HTTP_ENABLED", "true")
    .WithEnvironment("KC_HEALTH_ENABLED", "true")
    .WithEnvironment("KC_METRICS_ENABLED", "true")
    .WithEnvironment("KEYCLOAK_ADMIN", "admin")
    .WithEnvironment("KEYCLOAK_ADMIN_PASSWORD", "admin")
    .WithHttpEndpoint(port: 8180, targetPort: 8080, name: "http")
    .WithArgs("start-dev", "--import-realm")
    .WaitFor(postgres);

// Backend API
var backend = builder.AddProject<Projects.TrailmarksApi>("backend")
    .WithReference(trailmarksDb)
    .WithReference(keycloak)
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
    .WithHttpEndpoint(port: 8080, name: "http")
    .WaitFor(postgres)
    .WaitFor(keycloak);

// Frontend (Angular)
var frontend = builder.AddNpmApp("frontend", "../frontend", "start")
    .WithReference(backend)
    .WithEnvironment("API_URL", backend.GetEndpoint("http"))
    .WithHttpEndpoint(port: 4200, name: "http")
    .WaitFor(backend);

builder.Build().Run();
```

## Nächste Schritte

1. Issue im GitHub Repository erstellen
2. Team Review des Migrationsplans
3. Go/No-Go Entscheidung
4. Beginn Phase 1 der Implementation
