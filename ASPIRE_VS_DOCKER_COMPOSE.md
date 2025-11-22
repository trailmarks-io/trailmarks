# .NET Aspire vs. Docker Compose - Vergleich

## Quick Comparison

| Aspekt | Docker Compose | .NET Aspire |
|--------|----------------|-------------|
| **Konfiguration** | YAML-Dateien | C# Code |
| **Typsicherheit** | ❌ Keine | ✅ Compile-Time |
| **IDE Support** | ⚠️ Limitiert | ✅ Vollständig |
| **Debugging** | ⚠️ Container-basiert | ✅ Native .NET |
| **Observability** | ⚠️ Externe Tools | ✅ Integriert |
| **Service Discovery** | DNS-basiert | ✅ Automatisch |
| **Dashboard** | ❌ Kein natives | ✅ Aspire Dashboard |
| **Tracing** | Jaeger (separat) | ✅ Integriert |
| **Logs** | docker-compose logs | ✅ Dashboard |
| **Polyglot** | ✅ Ja | ⚠️ .NET-fokussiert |

## Entwickler-Workflows

### Lokale Entwicklung starten

**Docker Compose:**
```bash
# Terminal 1
docker-compose up -d

# Warten bis Services ready sind...
# Health Checks manuell prüfen
docker-compose ps

# Logs in separatem Terminal
docker-compose logs -f backend
```

**Aspire:**
```bash
# Alles in einem Befehl
dotnet run --project aspire/Trailmarks.AppHost

# Dashboard öffnet sich automatisch auf http://localhost:18888
# Zeigt: Logs, Traces, Metrics, Health Status - alles in einem UI
```

### Debugging

**Docker Compose:**
```bash
# Option 1: Logs verfolgen
docker-compose logs -f backend

# Option 2: In Container ausführen
docker-compose exec backend sh

# Option 3: Remote Debugging konfigurieren (komplex)
```

**Aspire:**
```
1. F5 in Visual Studio / VS Code drücken
2. Alle Services starten im Debug-Mode
3. Breakpoints funktionieren sofort
4. Aspire Dashboard zeigt alles in Echtzeit
```

### Traces ansehen

**Docker Compose:**
```
1. Browser öffnen → http://localhost:16686 (Jaeger)
2. Service auswählen
3. Traces suchen
4. Separate UI für Logs (docker-compose logs)
```

**Aspire:**
```
1. Aspire Dashboard öffnet automatisch
2. "Traces" Tab → Alle Traces
3. "Logs" Tab → Alle Logs
4. "Metrics" Tab → Performance
5. Alles in einem UI, Echtzeit-Updates
```

### Services verwalten

**Docker Compose:**
```bash
# Services neustarten
docker-compose restart backend

# Nur Infrastruktur starten
docker-compose up -d postgres keycloak jaeger

# Services skalieren
docker-compose up -d --scale backend=3

# Services stoppen
docker-compose down
```

**Aspire:**
```bash
# Alles neu starten
dotnet run --project aspire/Trailmarks.AppHost

# Einzelne Services in IDE kontrollieren
# Oder: AppHost Code anpassen und neu starten

# Services stoppen
Ctrl+C (stoppt alles koordiniert)
```

## Konfiguration im Vergleich

### PostgreSQL + PostGIS

**Docker Compose (YAML):**
```yaml
services:
  postgres:
    image: postgis/postgis:16-3.4-alpine
    container_name: trailmarks-postgres
    environment:
      POSTGRES_DB: trailmarks
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres
      TZ: Europe/Berlin
    volumes:
      - postgres-data:/var/lib/postgresql/data
    ports:
      - "5432:5432"
    networks:
      - trailmarks-network
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U postgres"]
      interval: 10s
      timeout: 5s
      retries: 5
    restart: unless-stopped
```

**Aspire (C#):**
```csharp
var postgres = builder.AddPostgres("postgres", password: "postgres", port: 5432)
    .WithImage("postgis/postgis", "16-3.4-alpine")
    .WithDataVolume()
    .WithEnvironment("TZ", "Europe/Berlin");

var trailmarksDb = postgres.AddDatabase("trailmarks");
var keycloakDb = postgres.AddDatabase("keycloak");
```

**Vorteile Aspire:**
- ✅ IntelliSense für alle Methoden
- ✅ Compile-Time Fehler wenn falsch
- ✅ Health Checks automatisch
- ✅ Typsicher, refactoring-freundlich

### Backend API

**Docker Compose (YAML):**
```yaml
backend:
  build:
    context: ./backend
    dockerfile: Dockerfile
  container_name: trailmarks-backend
  environment:
    ASPNETCORE_ENVIRONMENT: Production
    ASPNETCORE_URLS: http://+:8080
    ConnectionStrings__DefaultConnection: "Host=postgres;Port=5432;Database=trailmarks;..."
    UseSqlite: "false"
    OpenTelemetry__OtlpEndpoint: "http://jaeger:4317"
  ports:
    - "8080:8080"
  networks:
    - trailmarks-network
  depends_on:
    postgres:
      condition: service_healthy
    jaeger:
      condition: service_started
  restart: unless-stopped
```

**Aspire (C#):**
```csharp
var backend = builder.AddProject<Projects.TrailmarksApi>("backend")
    .WithReference(trailmarksDb)  // Connection String automatisch!
    .WithReference(keycloak)
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
    .WithHttpEndpoint(port: 8080, name: "http")
    .WaitFor(postgres)
    .WaitFor(keycloak);
```

**Vorteile Aspire:**
- ✅ Connection Strings automatisch injiziert
- ✅ Keine manuellen Environment-Variablen
- ✅ Dependencies typsicher
- ✅ Weniger Code, mehr Funktionalität

### Keycloak

**Docker Compose (YAML):**
```yaml
keycloak:
  image: quay.io/keycloak/keycloak:26.0.7
  container_name: trailmarks-keycloak
  command: start-dev --import-realm
  environment:
    KC_DB: postgres
    KC_DB_URL: jdbc:postgresql://postgres:5432/keycloak
    KC_DB_USERNAME: postgres
    KC_DB_PASSWORD: postgres
    # ... viele weitere Environment-Variablen
  volumes:
    - ./keycloak/realm-export.json:/opt/keycloak/data/import/realm-export.json:ro
  ports:
    - "8180:8080"
  networks:
    - trailmarks-network
  depends_on:
    postgres:
      condition: service_healthy
  healthcheck:
    test: ["CMD-SHELL", "exec 3<>/dev/tcp/localhost/8080 ..."]
    # ... komplexe Health Check Konfiguration
  restart: unless-stopped
```

**Aspire (C#):**
```csharp
var keycloak = builder.AddContainer("keycloak", "quay.io/keycloak/keycloak", "26.0.7")
    .WithBindMount("../keycloak/realm-export.json", "/opt/keycloak/data/import/realm-export.json", isReadOnly: true)
    .WithEnvironment("KC_DB", "postgres")
    .WithEnvironment("KC_DB_URL", "jdbc:postgresql://postgres:5432/keycloak")
    // ... weitere Environments (gleich wie vorher)
    .WithHttpEndpoint(port: 8180, targetPort: 8080, name: "http")
    .WithArgs("start-dev", "--import-realm")
    .WaitFor(postgres);
```

**Vorteile Aspire:**
- ✅ Gleiche Funktionalität, weniger YAML
- ✅ Health Checks automatisch
- ✅ Typsichere Port-Konfiguration
- ✅ Dependencies klar definiert

## Observability: Jaeger vs. Aspire Dashboard

### Jaeger (Docker Compose Setup)

**Services benötigt:**
- Jaeger Container (Port 16686 UI, 4317 OTLP gRPC, 4318 OTLP HTTP)
- NGINX OTLP Proxy (Port 6060) - für CORS von Frontend

**Funktionen:**
- ✅ Distributed Tracing
- ❌ Keine Logs
- ❌ Keine Metrics
- ⚠️ Separate UI

**Zugriff:**
- Jaeger UI: http://localhost:16686
- Logs: `docker-compose logs -f`
- Metrics: Nicht verfügbar

### Aspire Dashboard

**Services benötigt:**
- Nichts! Automatisch included

**Funktionen:**
- ✅ Distributed Tracing
- ✅ Strukturierte Logs
- ✅ Metrics
- ✅ Health Checks
- ✅ Service Dependencies
- ✅ Alles in einer UI

**Zugriff:**
- Dashboard: http://localhost:18888 (automatisch öffnet)
- Tabs: Resources, Console, Logs, Traces, Metrics

## Was entfällt mit Aspire?

### Nicht mehr benötigt

1. **Jaeger Container**
   - Ersetzt durch Aspire Dashboard
   - Tracing funktioniert automatisch

2. **NGINX OTLP Proxy**
   - Nicht mehr nötig
   - Aspire Dashboard hat CORS Support

3. **docker-compose.yml**
   - Ersetzt durch C# AppHost
   - Typsicher und wartbar

4. **Separate Logs-Tools**
   - Aspire Dashboard zeigt alles

### Bleibt gleich

1. **PostgreSQL Container**
   - Läuft weiter als Container
   - Via Aspire Component

2. **Keycloak Container**
   - Läuft weiter als Container
   - Via Container Component

3. **Frontend Build**
   - Kann als Container oder npm app laufen
   - Gleiche Funktionalität

## Performance-Vergleich

| Metrik | Docker Compose | Aspire |
|--------|----------------|--------|
| **Startup Zeit** | ~30-60s | ~20-40s |
| **Memory Overhead** | ~500MB (Jaeger + NGINX) | ~200MB (Dashboard) |
| **Developer Experience** | ⭐⭐⭐ | ⭐⭐⭐⭐⭐ |
| **Debugging** | ⭐⭐ | ⭐⭐⭐⭐⭐ |
| **Observability** | ⭐⭐⭐ | ⭐⭐⭐⭐⭐ |

## Migration Complexity

### Einfache Migration

- ✅ Backend (.NET Projekt) → Sehr einfach
- ✅ PostgreSQL → Einfach (Aspire Component)
- ✅ Keycloak → Mittel (Container Component)
- ✅ Frontend → Mittel (npm oder Container)

### Timeline

- **Phase 1:** Vorbereitung (2 Tage)
- **Phase 2:** Backend (3 Tage)
- **Phase 3:** Frontend (2 Tage)
- **Phase 4:** Keycloak & PostGIS (2 Tage)
- **Phase 5:** Cleanup & Docs (2 Tage)

**Gesamt: ~2 Wochen**

## Wann Aspire, wann Docker Compose?

### Nutze .NET Aspire wenn:
- ✅ Du hauptsächlich .NET entwickelst
- ✅ Du bessere Developer Experience willst
- ✅ Du integrierte Observability brauchst
- ✅ Du typsichere Konfiguration bevorzugst
- ✅ Du Visual Studio / VS Code nutzt

### Nutze Docker Compose wenn:
- ✅ Du polyglot Stack hast (viele Sprachen)
- ✅ Du kein .NET nutzt
- ✅ Du nur Container orchestrieren willst
- ✅ Du YAML bevorzugst
- ✅ Du sprachunabhängig bleiben willst

## Für Trailmarks

**Empfehlung: Migration zu Aspire** ✅

**Gründe:**
- Backend ist .NET 9.0 → Native Integration
- Aspire Dashboard besser als Jaeger + separate Logs
- Bessere Developer Experience für das Team
- Typsichere Konfiguration reduziert Fehler
- Modernere Technologie für Cloud-Native Apps
- NGINX OTLP Proxy wird überflüssig

**Keine Nachteile:**
- Frontend (Angular) läuft weiter als Container/npm
- PostgreSQL läuft weiter (via Aspire Component)
- Keycloak läuft weiter (via Container Component)
- Gleiche Funktionalität, bessere Tools

## Zusammenfassung

.NET Aspire ist der **logische nächste Schritt** für das Trailmarks-Projekt:

| Bereich | Verbesserung |
|---------|--------------|
| **Developer Experience** | 🚀 Deutlich besser |
| **Observability** | 📊 Alles in einem Dashboard |
| **Configuration** | 🎯 Typsicher und wartbar |
| **Debugging** | 🐛 Native .NET Debugging |
| **Startup** | ⚡ Schneller |
| **Complexity** | 📉 Reduziert |

**Bottom Line:** Alle Vorteile, keine Nachteile für .NET-basierte Projekte wie Trailmarks.
