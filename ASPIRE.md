# .NET Aspire Deployment Guide

This guide explains how to run the Trailmarks application using .NET Aspire.

## Prerequisites

- .NET 9.0 SDK or higher
- Docker (for PostgreSQL and Keycloak containers)
- No local installation of PostgreSQL or Keycloak required

## Quick Start

### 1. Clone the Repository

```bash
git clone https://github.com/trailmarks-io/trailmarks.git
cd trailmarks
```

### 2. Start All Services with Aspire

```bash
dotnet run --project aspire/Trailmarks.AppHost
```

This command will:
- Start the Aspire Dashboard
- Start PostgreSQL database with PostGIS
- Start Keycloak authentication server
- Build and run the backend API
- Initialize the database with sample data (on first run)
- Open the Aspire Dashboard in your browser

### 3. Access the Application

- **Aspire Dashboard**: http://localhost:18888 (opens automatically)
- **Frontend**: http://localhost:4200 (if integrated)
- **Backend API**: http://localhost:8080
- **API Documentation (Swagger)**: http://localhost:8080/swagger
- **Keycloak Admin Console**: http://localhost:8180

### 4. Stop All Services

Press `Ctrl+C` in the terminal where Aspire is running. This will gracefully stop all services.

## Aspire Dashboard

The Aspire Dashboard provides a unified view of all your services:

### Dashboard Tabs

| Tab | Description |
|-----|-------------|
| **Resources** | View status of all services (Backend, PostgreSQL, Keycloak) with health checks |
| **Console** | See console output from all services in real-time |
| **Logs** | Structured logs from all services with filtering and search |
| **Traces** | Distributed tracing (replaces Jaeger UI) |
| **Metrics** | Performance metrics from all services |

### Features

- ✅ **Unified Observability** - Logs, Traces, and Metrics in one place
- ✅ **Real-time Updates** - See changes as they happen
- ✅ **Service Health** - Visual indicators for service status
- ✅ **Easy Navigation** - Switch between different services quickly
- ✅ **No Configuration** - Works out of the box

## Services Overview

### Backend API (TrailmarksApi)

- **Container Name**: TrailmarksApi
- **Port**: 8080
- **Technology**: .NET 9.0 ASP.NET Core
- **Features**: REST API, Entity Framework Core, OpenTelemetry
- **Service Defaults**: Includes telemetry, health checks, service discovery

### PostgreSQL with PostGIS

- **Container Name**: postgres
- **Port**: 5432 (internal)
- **Version**: PostgreSQL 16 Alpine with PostGIS 3.4
- **Databases**: `trailmarks`, `keycloak`
- **Data Persistence**: Docker volume

### Keycloak

- **Container Name**: keycloak
- **Port**: 8180 (mapped to internal port 8080)
- **Version**: Keycloak 26.0.7
- **Realm**: `trailmarks` (auto-imported on startup)
- **Admin Credentials**: admin / admin

## Development Workflows

### Running Specific Services

If you want to run services individually for development:

```bash
# Backend only (requires PostgreSQL running separately)
cd backend/src/TrailmarksApi
dotnet run

# Frontend only (requires Backend running)
cd frontend
npm install
npx ng serve
```

### Debugging

**In Visual Studio / VS Code:**
1. Open `aspire/Trailmarks.AppHost/Trailmarks.AppHost.csproj`
2. Press F5 to start debugging
3. Set breakpoints in your backend code
4. Breakpoints will work immediately when requests are made

### Viewing Logs

**Option 1: Aspire Dashboard**
- Navigate to http://localhost:18888
- Click on "Logs" tab
- Filter by service or log level

**Option 2: Console Output**
- Aspire shows all service logs in the terminal where it's running
- Color-coded by service

### Viewing Traces

1. Open Aspire Dashboard: http://localhost:18888
2. Click "Traces" tab
3. Select "TrailmarksApi" service
4. Click on any trace to see detailed timing breakdown
5. See which operations are slow and optimize accordingly

## Configuration

### Passwords and Secrets

Aspire uses user secrets for sensitive data. Set the PostgreSQL password:

```bash
cd aspire/Trailmarks.AppHost
dotnet user-secrets set "Parameters:postgres-password" "your_secure_password"
```

Default password is "postgres" (for local development).

### Customizing Ports

To change ports, edit `aspire/Trailmarks.AppHost/AppHost.cs`:

```csharp
// Change Backend API port
var backend = builder.AddProject<Projects.TrailmarksApi>("backend")
    .WithHttpEndpoint(port: 5000, name: "http");  // Changed from 8080
```

## Troubleshooting

### Services Not Starting

1. Check Aspire Dashboard → Resources tab for service status
2. Look at Console or Logs tab for error messages
3. Ensure Docker is running (for PostgreSQL and Keycloak)
4. Check if ports 8080, 8180, 5432, 18888 are available

### Database Connection Issues

1. Verify PostgreSQL container is running in Aspire Dashboard
2. Check the connection string is correct
3. Ensure database initialization completed (check logs)

### Keycloak Not Accessible

1. Check Keycloak status in Aspire Dashboard → Resources
2. Wait 60-90 seconds for Keycloak to fully start
3. Access Admin Console: http://localhost:8180
4. Verify realm import succeeded (should see "trailmarks" realm)

### Port Conflicts

If ports are already in use:
1. Stop other services using those ports
2. Or edit AppHost.cs to use different ports

## Comparing with Docker Compose

### What Changed

| Aspect | Docker Compose | .NET Aspire |
|--------|----------------|-------------|
| **Start Command** | `docker-compose up -d` | `dotnet run --project aspire/Trailmarks.AppHost` |
| **Dashboard** | Jaeger (http://localhost:16686) | Aspire Dashboard (http://localhost:18888) |
| **Logs** | `docker-compose logs -f` | Aspire Dashboard → Logs tab |
| **Configuration** | YAML files | C# code (type-safe) |
| **Debugging** | Remote debugging setup needed | Native .NET debugging (F5) |

### Advantages of Aspire

- ✅ **One Dashboard** - Logs, Traces, Metrics all in one place (instead of separate tools)
- ✅ **Type-Safe Configuration** - Compile-time validation of service configuration
- ✅ **Better Developer Experience** - Integrated debugging, IntelliSense support
- ✅ **Automatic Service Discovery** - Services find each other automatically
- ✅ **Health Checks** - Visual service health in dashboard

### What Stayed the Same

- ✅ PostgreSQL and Keycloak still run as containers
- ✅ Same ports and URLs
- ✅ Same functionality
- ✅ Same data persistence

## Additional Resources

- [.NET Aspire Documentation](https://learn.microsoft.com/en-us/dotnet/aspire/)
- [Aspire Dashboard Guide](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/dashboard)
- [OpenTelemetry with Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/telemetry)

## Migration from Docker Compose

If you still need Docker Compose for any reason, it's available as `docker-compose.yml.deprecated`. However, .NET Aspire is now the recommended way to run the application.

To use the old Docker Compose setup:
```bash
# Rename file back
mv docker-compose.yml.deprecated docker-compose.yml

# Start with Docker Compose
docker-compose up -d
```

> **Note:** Docker Compose support is deprecated and may be removed in future versions. Please migrate to .NET Aspire.
