// Beispiel-Code für Trailmarks.AppHost/Program.cs
// Dies ist ein Beispiel, wie der finale AppHost Code aussehen könnte

using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// ============================================================================
// PostgreSQL mit PostGIS Extension
// ============================================================================
var postgres = builder.AddPostgres("postgres", password: "postgres", port: 5432)
    .WithImage("postgis/postgis", "16-3.4-alpine")  // PostGIS Image statt Standard
    .WithDataVolume()  // Persistente Daten
    .WithEnvironment("TZ", "Europe/Berlin");

// Datenbanken erstellen
var trailmarksDb = postgres.AddDatabase("trailmarks");
var keycloakDb = postgres.AddDatabase("keycloak");

// ============================================================================
// Keycloak - Authentication & Authorization
// ============================================================================
var keycloak = builder.AddContainer("keycloak", "quay.io/keycloak/keycloak", "26.0.7")
    // Volume Mounts für Konfiguration
    .WithBindMount("../keycloak/realm-export.json", 
                   "/opt/keycloak/data/import/realm-export.json", 
                   isReadOnly: true)
    .WithBindMount("../keycloak/init-keycloak-db.sh", 
                   "/docker-entrypoint-initdb.d/init-keycloak-db.sh", 
                   isReadOnly: true)
    
    // Datenbank-Konfiguration
    .WithEnvironment("KC_DB", "postgres")
    .WithEnvironment("KC_DB_URL", "jdbc:postgresql://postgres:5432/keycloak")
    .WithEnvironment("KC_DB_USERNAME", "postgres")
    .WithEnvironment("KC_DB_PASSWORD", "postgres")
    
    // Hostname-Konfiguration
    .WithEnvironment("KC_HOSTNAME", "localhost")
    .WithEnvironment("KC_HOSTNAME_PORT", "8180")
    .WithEnvironment("KC_HOSTNAME_STRICT", "false")
    .WithEnvironment("KC_HOSTNAME_STRICT_HTTPS", "false")
    
    // Features aktivieren
    .WithEnvironment("KC_HTTP_ENABLED", "true")
    .WithEnvironment("KC_HEALTH_ENABLED", "true")
    .WithEnvironment("KC_METRICS_ENABLED", "true")
    
    // Admin-Credentials
    .WithEnvironment("KEYCLOAK_ADMIN", "admin")
    .WithEnvironment("KEYCLOAK_ADMIN_PASSWORD", "admin")
    
    // Port Mapping und Startup Command
    .WithHttpEndpoint(port: 8180, targetPort: 8080, name: "http")
    .WithArgs("start-dev", "--import-realm")
    
    // Warten bis PostgreSQL bereit ist
    .WaitFor(postgres);

// ============================================================================
// Backend API - .NET 9.0 ASP.NET Core
// ============================================================================
var backend = builder.AddProject<Projects.TrailmarksApi>("backend")
    // Service-Referenzen (automatische Connection String Injection)
    .WithReference(trailmarksDb)
    .WithReference(keycloak)
    
    // Environment-Konfiguration
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
    
    // HTTP Endpoint
    .WithHttpEndpoint(port: 8080, name: "http")
    
    // Startup-Reihenfolge
    .WaitFor(postgres)
    .WaitFor(keycloak);

// ============================================================================
// Frontend - Angular 20.1
// ============================================================================
// Option A: Als npm App (für Development)
var frontend = builder.AddNpmApp("frontend", "../frontend", "start")
    .WithReference(backend)  // Backend-URL wird automatisch injiziert
    .WithHttpEndpoint(port: 4200, name: "http")
    .WaitFor(backend);

// Option B: Als Container (für Production-ähnliches Setup)
/*
var frontend = builder.AddContainer("frontend", "node", "20-alpine")
    .WithBindMount("../frontend", "/app")
    .WithWorkingDirectory("/app")
    .WithEntrypoint("sh", "-c", "npm install && npm start")
    .WithHttpEndpoint(port: 4200, name: "http")
    .WithEnvironment("API_URL", backend.GetEndpoint("http"))
    .WithReference(backend)
    .WaitFor(backend);
*/

// ============================================================================
// Build und Run
// ============================================================================
builder.Build().Run();

// Das war's! 🎉
// 
// Aspire kümmert sich automatisch um:
// - Service Discovery
// - Connection String Injection
// - OpenTelemetry Configuration
// - Health Checks
// - Aspire Dashboard (http://localhost:18888)
//
// Entfallen:
// ❌ Jaeger (ersetzt durch Aspire Dashboard)
// ❌ NGINX OTLP Proxy (nicht mehr nötig)
// ❌ YAML-Konfiguration (alles in C#)
//
// Gewinne:
// ✅ Typsichere Konfiguration
// ✅ IntelliSense Support
// ✅ Compile-Time Validierung
// ✅ Ein Dashboard für alles (Logs, Traces, Metrics)
// ✅ Besseres Debugging
