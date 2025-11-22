var builder = DistributedApplication.CreateBuilder(args);

// PostgreSQL with PostGIS
var postgresPassword = builder.AddParameter("postgres-password", secret: true);
var postgres = builder.AddPostgres("postgres", postgresPassword, port: 5432)
    .WithImage("postgis/postgis", "16-3.4-alpine")
    .WithDataVolume()
    .WithEnvironment("TZ", "Europe/Berlin");

var trailmarksDb = postgres.AddDatabase("trailmarks");
var keycloakDb = postgres.AddDatabase("keycloak");

// Keycloak - Authentication and Authorization
var keycloak = builder.AddContainer("keycloak", "quay.io/keycloak/keycloak", "26.0.7")
    .WithBindMount("../../keycloak/realm-export.json", "/opt/keycloak/data/import/realm-export.json", isReadOnly: true)
    .WithBindMount("../../keycloak/init-keycloak-db.sh", "/docker-entrypoint-initdb.d/init-keycloak-db.sh", isReadOnly: true)
    .WithEnvironment("KC_DB", "postgres")
    .WithEnvironment("KC_DB_URL", "jdbc:postgresql://postgres:5432/keycloak")
    .WithEnvironment("KC_DB_USERNAME", "postgres")
    .WithEnvironment("KC_DB_PASSWORD", postgresPassword)
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
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
    .WithHttpEndpoint(port: 8080, name: "http")
    .WaitFor(postgres)
    .WaitFor(keycloak);

// Frontend (Angular)
var frontend = builder.AddNpmApp("frontend", "../../frontend", "start")
    .WithHttpEndpoint(port: 4200, env: "PORT")
    .WithExternalHttpEndpoints()
    .WaitFor(backend);

builder.Build().Run();
