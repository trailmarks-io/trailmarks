# Migration von Docker Compose zu .NET Aspire

## 🎯 Ziel

Migration des Trailmarks-Projekts von Docker Compose zu .NET Aspire, um die Entwicklererfahrung zu verbessern und die Orchestrierung zu modernisieren. Das Aspire Dashboard soll Jaeger als Observability-Tool ablösen.

## 📋 Motivation

### Warum .NET Aspire?

**Entwicklererfahrung:**
- ✅ Typsichere C#-Konfiguration statt YAML
- ✅ IntelliSense und Compile-Time-Validierung
- ✅ Integriertes Debugging für alle Services
- ✅ Aspire Dashboard für Observability

**Vereinfachung:**
- ✅ Automatische Service Discovery
- ✅ Integrierte OpenTelemetry-Konfiguration
- ✅ Health Checks out-of-the-box
- ✅ Keine separaten NGINX-Proxies für OTLP nötig

**Modernisierung:**
- ✅ Native .NET-Integration
- ✅ Cloud-Ready Architektur
- ✅ Reduzierung von YAML-Konfiguration
- ✅ Bessere IDE-Unterstützung

## 🏗️ Architektur

### Aktuell (Docker Compose)

```
Services:
- Jaeger (Port 16686, 4317, 4318) - OpenTelemetry Backend
- NGINX OTLP Proxy (Port 6060) - CORS für Frontend
- PostgreSQL + PostGIS (Port 5432) - Datenbank
- Keycloak (Port 8180) - Authentication
- Backend API (Port 8080) - .NET 9.0 ASP.NET Core
- Frontend (Port 4200) - Angular 20.1
```

### Ziel (.NET Aspire)

```
Neue Struktur:
- Trailmarks.AppHost (Orchestrierung in C#)
- TrailmarksApi.ServiceDefaults (Shared Configuration)
- Aspire Dashboard (Port 18888) - Ersetzt Jaeger + Logs
- PostgreSQL + PostGIS - Via Aspire Component
- Keycloak - Via Container Component
- Backend API - Als .NET Projekt
- Frontend - Als Container/npm Component
```

**Vereinfachungen:**
- ❌ Jaeger entfällt (ersetzt durch Aspire Dashboard)
- ❌ NGINX OTLP Proxy entfällt (nicht mehr nötig)
- ✅ Ein Dashboard für Logs, Traces, Metrics
- ✅ Typsichere Konfiguration

## 📝 Implementierungs-Plan

Detaillierter Migrationsplan siehe: [ASPIRE_MIGRATION_PLAN.md](ASPIRE_MIGRATION_PLAN.md)

### Phase 1: Vorbereitung (2 Tage)
**Ziel:** Aspire-Struktur parallel zu Docker Compose aufbauen

**Tasks:**
- [ ] Aspire Templates installieren (bereits erledigt ✅)
- [ ] AppHost Projekt erstellen (`aspire/Trailmarks.AppHost`)
- [ ] ServiceDefaults Projekt erstellen (`backend/src/TrailmarksApi.ServiceDefaults`)
- [ ] Solution-Datei aktualisieren
- [ ] Projektstruktur verifizieren

**Ergebnis:** Aspire-Struktur existiert, Docker Compose funktioniert weiter

---

### Phase 2: Backend Migration (3 Tage)
**Ziel:** Backend-API in Aspire integrieren

**Tasks:**
- [ ] TrailmarksApi auf ServiceDefaults umstellen
  - [ ] ServiceDefaults Paket referenzieren
  - [ ] `AddServiceDefaults()` zu Program.cs hinzufügen
  - [ ] OpenTelemetry Config nach ServiceDefaults verschieben
- [ ] AppHost konfigurieren
  - [ ] PostgreSQL mit PostGIS hinzufügen
  - [ ] Backend Projekt hinzufügen
  - [ ] Connection Strings konfigurieren
- [ ] Backend mit Aspire starten testen
- [ ] OpenTelemetry zu Aspire Dashboard verifizieren

**Ergebnis:** Backend läuft mit Aspire Dashboard, zeigt Traces

---

### Phase 3: Frontend Integration (2 Tage)
**Ziel:** Angular Frontend in Aspire integrieren

**Tasks:**
- [ ] Frontend Integration evaluieren (Container vs. npm)
- [ ] Frontend zu AppHost hinzufügen
- [ ] Backend-URL via Environment Variable konfigurieren
- [ ] OTLP Endpoint auf Aspire Dashboard umleiten
- [ ] Frontend Build und Start testen

**Ergebnis:** Vollständige Anwendung läuft in Aspire

---

### Phase 4: Keycloak und PostGIS (2 Tage)
**Ziel:** Vollständige Feature-Parität mit Docker Compose

**Tasks:**
- [ ] PostgreSQL mit PostGIS Image konfigurieren
- [ ] Keycloak als Container Component hinzufügen
  - [ ] Realm Export Volume Mount
  - [ ] Init Script Volume Mount
  - [ ] Environment Variables konfigurieren
  - [ ] Port Mapping (8180:8080)
- [ ] Keycloak Database Connection konfigurieren
- [ ] Startup-Reihenfolge mit `WaitFor()` sicherstellen
- [ ] Health Checks verifizieren
- [ ] Keycloak Realm Import testen

**Ergebnis:** Alle Services funktionieren wie zuvor

---

### Phase 5: Cleanup und Dokumentation (2 Tage)
**Ziel:** Aufräumen und Dokumentation aktualisieren

**Tasks:**
- [ ] Docker Compose deprecaten oder entfernen
  - [ ] `docker-compose.yml` umbenennen zu `.deprecated`
  - [ ] Oder: Als Fallback mit Hinweis behalten
- [ ] NGINX OTLP Config entfernen
  - [ ] `nginx-otlp.conf` löschen
  - [ ] Nginx Service aus docker-compose entfernen
- [ ] Dokumentation aktualisieren
  - [ ] `README.md`: Aspire Quickstart
  - [ ] `DOCKER.md` → `ASPIRE.md`
  - [ ] Neue Developer Workflows dokumentieren
- [ ] GitHub Workflows anpassen
  - [ ] CI/CD auf Aspire umstellen
  - [ ] Build-Prozesse aktualisieren
- [ ] Team Onboarding vorbereiten

**Ergebnis:** Sauberes Repository mit Aspire als Standard

---

## 🎨 Neue Developer Experience

### Vorher (Docker Compose)
```bash
# Starten
docker-compose up -d

# Logs anschauen
docker-compose logs -f backend

# Traces anschauen
# Browser → http://localhost:16686 (Jaeger)

# Services debuggen
# Manuell attach oder logs folgen
```

### Nachher (Aspire)
```bash
# Starten (alles in einem)
dotnet run --project aspire/Trailmarks.AppHost

# Dashboard öffnet automatisch
# Browser → http://localhost:18888

# Features:
# ✅ Logs von allen Services
# ✅ Traces (OpenTelemetry)
# ✅ Metrics
# ✅ Service Health Status
# ✅ Dependencies Visualisierung
```

### Debugging
- F5 in Visual Studio / VS Code startet alle Services
- Breakpoints in Backend funktionieren sofort
- Logs, Traces, Metrics in einem Dashboard

## 🎁 Aspire Dashboard Features

Das Aspire Dashboard ersetzt **mehrere Tools** auf einmal:

| Vorher | Nachher (Aspire Dashboard) |
|--------|----------------------------|
| Jaeger UI (Traces) | ✅ Traces Tab |
| docker-compose logs | ✅ Logs Tab (strukturiert) |
| Health Checks manuell | ✅ Resources Tab (Status) |
| Metrics manuell sammeln | ✅ Metrics Tab |
| Service Dependencies | ✅ Visual Dependencies |

**Features:**
- 🔍 Verteilte Tracing (OpenTelemetry)
- 📝 Strukturierte Logs mit Filtering & Search
- 📊 Metrics und Performance-Daten
- 🔗 Service-Abhängigkeiten visualisiert
- ❤️ Health Status aller Services
- ⚡ Echtzeit-Updates
- 🚀 Keine separate Installation nötig

## ⚠️ Risiken und Mitigations

| Risiko | Mitigation |
|--------|------------|
| Frontend ist kein .NET Projekt | Container-based Integration (wie Docker Compose) |
| Keycloak Komplexität | Container Component mit Volume Mounts wie bisher |
| PostGIS Extension | Custom Image: `postgis/postgis:16-3.4-alpine` |
| Team Adoption | Umfassende Docs, Docker Compose als Fallback |

## ✅ Erfolgs-Kriterien

- [ ] Alle Services laufen mit Aspire
- [ ] Aspire Dashboard zeigt Traces, Logs, Metrics korrekt
- [ ] Backend-Tests (xUnit) bestehen alle
- [ ] Frontend-Tests (Jasmine/Karma) bestehen alle
- [ ] E2E-Tests (Playwright) bestehen alle
- [ ] Dokumentation vollständig aktualisiert
- [ ] Team kann mit Aspire arbeiten
- [ ] Performance mindestens gleich gut wie vorher

## 📚 Dokumentation

**Detaillierter Migrationsplan:**
- [ASPIRE_MIGRATION_PLAN.md](ASPIRE_MIGRATION_PLAN.md)

**Referenzen:**
- [Microsoft Learn: Aspire Overview](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview)
- [Microsoft Learn: Migrate from Docker Compose](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/migrate-from-docker-compose)
- [Aspire Dashboard Documentation](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/dashboard)

## ⏱️ Timeline

| Phase | Dauer | Abhängigkeiten |
|-------|-------|----------------|
| Phase 1: Vorbereitung | 2 Tage | - |
| Phase 2: Backend Migration | 3 Tage | Phase 1 |
| Phase 3: Frontend Integration | 2 Tage | Phase 2 |
| Phase 4: Keycloak & PostGIS | 2 Tage | Phase 2 |
| Phase 5: Cleanup & Docs | 2 Tage | Phase 3 & 4 |
| **Gesamt** | **~2 Wochen** | - |

## 🔖 Labels

- `enhancement`
- `infrastructure`
- `aspire`
- `docker`
- `observability`

## 👥 Team Review

**Benötigt Feedback zu:**
1. Zeitplan realistisch?
2. Docker Compose entfernen oder als Fallback behalten?
3. Frontend Integration: Container oder npm Executable?
4. Breaking Changes akzeptabel?
5. Migration in einem Rutsch oder schrittweise?

---

**Related Issues:**
- Keine (neue Initiative)

**Related PRs:**
- Wird erstellt nach Approval dieses Issues
