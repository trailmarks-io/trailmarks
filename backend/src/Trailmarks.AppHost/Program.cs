var builder = DistributedApplication.CreateBuilder(args);

// Define a parameter for the postgres password
var postgresPassword = builder.AddParameter("postgres-password", secret: true);

// PostgreSQL database with PostGIS extension
var postgres = builder.AddPostgres("postgres", password: postgresPassword)
    .WithImage("postgis/postgis", "16-3.4-alpine")
    .WithEnvironment("TZ", "Europe/Berlin")
    .WithDataVolume("postgres-data")
    .WithPgAdmin();

var trailmarksDb = postgres.AddDatabase("trailmarks");
var keycloakDb = postgres.AddDatabase("keycloak");

// Keycloak authentication server (using container since no stable Aspire package)
var keycloak = builder.AddContainer("keycloak", "quay.io/keycloak/keycloak", "26.0.7")
    .WithBindMount("../../../keycloak/realm-export.json", "/opt/keycloak/data/import/realm-export.json", isReadOnly: true)
    .WithEnvironment("KC_DB", "postgres")
    .WithEnvironment("KC_DB_URL", keycloakDb.Resource.ConnectionStringExpression)
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
    .WaitFor(keycloakDb);

// Backend API
var backend = builder.AddProject<Projects.TrailmarksApi>("backend")
    .WithReference(trailmarksDb)
    .WaitFor(trailmarksDb)
    .WithEnvironment("UseSqlite", "false")
    .WithArgs("-DbInit");

// Frontend (Angular application)
var frontend = builder.AddNpmApp("frontend", "../../../frontend")
    .WithReference(backend)
    .WithEnvironment("API_URL", backend.GetEndpoint("http"))
    .WithHttpEndpoint(port: 4200, targetPort: 4200, env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
