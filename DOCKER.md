# Docker Deployment Guide

This guide explains how to deploy the Trailmarks application using .NET Aspire for local development or Docker Compose for production deployment.

## Prerequisites

- Docker Engine 20.10 or higher
- Docker Compose V2 or higher
- .NET SDK 9.0 or higher (for Aspire development)

## .NET Aspire (Recommended for Development)

.NET Aspire provides a modern orchestration experience with built-in observability, service discovery, and the Aspire Dashboard for traces, logs, and metrics.

### Quick Start with Aspire

1. **Navigate to the AppHost project**:
   ```bash
   cd backend/src/Trailmarks.AppHost
   ```

2. **Run the application**:
   ```bash
   dotnet run
   ```

3. **Access the services**:
   - **Aspire Dashboard**: Opens automatically in browser (usually https://localhost:17110)
   - **Frontend**: http://localhost:4200
   - **Backend API**: http://localhost:8080
   - **API Documentation (Swagger)**: http://localhost:8080/swagger
   - **Keycloak Admin Console**: http://localhost:8180
   - **PgAdmin**: Available through the dashboard

### Aspire Dashboard Features

The Aspire Dashboard replaces Jaeger and provides:
- **Traces**: Distributed tracing across all services
- **Metrics**: Runtime and HTTP metrics
- **Logs**: Structured logging from all services
- **Resources**: Health and status of all orchestrated services

### Services Orchestrated by Aspire

- **PostgreSQL with PostGIS**: Database with spatial data support
- **Keycloak**: Authentication and authorization server
- **Backend API**: ASP.NET Core Web API
- **Frontend**: Angular application
- **PgAdmin**: Database administration tool

## Docker Compose (Alternative/Production)

### 1. Clone the Repository

```bash
git clone https://github.com/trailmarks-io/trailmarks.git
cd trailmarks
```

### 2. Start All Services

```bash
docker-compose up -d
```

This command will:
- Build the backend Docker image from source
- Build the frontend Docker image from source
- Download and start PostgreSQL database
- Initialize the database with sample data
- Start all services in the background

### 3. Access the Application

- **Frontend**: http://localhost:4200
- **Backend API**: http://localhost:8080
- **API Documentation (Swagger)**: http://localhost:8080/swagger
- **Keycloak Admin Console**: http://localhost:8180

> **Note**: The docker-compose setup uses the legacy Jaeger tracing. For the best development experience with modern observability features, use the .NET Aspire setup instead.

### 4. Stop All Services

```bash
docker-compose down
```

To also remove the database volume:

```bash
docker-compose down -v
```

## Services Overview

### Frontend (Angular)
- **Container Name**: `trailmarks-frontend`
- **Port**: 4200 (mapped to internal port 80)
- **Technology**: Angular 20.1.0 served by nginx
- **Build**: Multi-stage build (Node.js for building, nginx for serving)

### Backend (ASP.NET Core)
- **Container Name**: `trailmarks-backend`
- **Port**: 8080
- **Technology**: .NET 8.0 ASP.NET Core
- **Database**: PostgreSQL (only)
- **Build**: Multi-stage build (.NET SDK for building, ASP.NET runtime for execution)

### Database (PostgreSQL with PostGIS)
- **Container Name**: `trailmarks-postgres`
- **Port**: 5432 (internal only, not exposed to host)
- **Version**: PostgreSQL 16 Alpine with PostGIS 3.4
- **Image**: `postgis/postgis:16-3.4-alpine`
- **Extensions**: PostGIS for spatial data support
- **Data Persistence**: Docker volume `postgres-data`
- **Databases**:
  - `trailmarks` - Main application database
  - `keycloak` - Keycloak authentication database (auto-created on first start)
- **Credentials**:
  - User: `postgres`
  - Password: `postgres`

### Authentication (Keycloak)
- **Container Name**: `trailmarks-keycloak`
- **Port**: 8180 (mapped to internal port 8080)
- **Version**: Keycloak 26.0.7
- **Image**: `quay.io/keycloak/keycloak:26.0.7`
- **Database**: Uses PostgreSQL `keycloak` database
- **Admin Console**: http://localhost:8180
- **Admin Credentials**:
  - Username: `admin`
  - Password: `admin`
- **Realm**: `trailmarks` (auto-imported on startup)
- **Clients**:
  - `trailmarks-frontend` - Public client for Angular SPA
  - `trailmarks-backend` - Bearer-only client for API protection
- **Features**:
  - OpenID Connect (OIDC) authentication
  - OAuth2 authorization
  - User management
  - Role-based access control (user, moderator, admin)
  - Multi-language support (English, German)
  - Self-registration enabled
  - Password reset enabled
  - Brute force protection

### Observability (Aspire Dashboard)

When using .NET Aspire, the Aspire Dashboard provides comprehensive observability:

- **Dashboard URL**: Opens automatically when running `dotnet run` in AppHost
- **Features**:
  - Distributed tracing across all services
  - Structured logging with search and filtering
  - Runtime metrics and HTTP metrics
  - Service health monitoring
  - Resource status overview

### Observability (Jaeger - Docker Compose only)
- **Container Name**: `trailmarks-jaeger`
- **Port**: 16686 (Jaeger UI), 4318 (internal OTLP receiver)
- **Technology**: Jaeger all-in-one with OpenTelemetry support
- **Purpose**: Distributed tracing and performance monitoring (legacy)
- **Features**:
  - Trace visualization
  - Service dependency graph
  - Performance analysis
  - Request flow tracking

> **Recommendation**: Use .NET Aspire for development as it provides a more integrated experience with modern observability features.

### OTLP Proxy (NGINX)
- **Container Name**: `trailmarks-nginx-otlp`
- **Port**: 4318 (OTLP HTTP proxy with CORS)
- **Technology**: NGINX Alpine
- **Purpose**: Proxy OTLP requests from frontend to Jaeger with CORS support
- **Features**:
  - 1:1 forwarding to Jaeger OTLP endpoint
  - CORS headers for browser requests
  - Support for OPTIONS preflight requests

## Docker Commands

### View Running Containers

```bash
docker-compose ps
```

### View Logs

```bash
# All services
docker-compose logs -f

# Specific service
docker-compose logs -f backend
docker-compose logs -f frontend
docker-compose logs -f postgres
docker-compose logs -f keycloak
docker-compose logs -f jaeger
docker-compose logs -f nginx-otlp
```

### Restart Services

```bash
# Restart all services
docker-compose restart

# Restart specific service
docker-compose restart backend
```

### Rebuild After Code Changes

```bash
# Rebuild and restart all services
docker-compose up -d --build

# Rebuild specific service
docker-compose up -d --build backend
```

### Execute Commands Inside Containers

```bash
# Backend shell
docker-compose exec backend sh

# Database shell
docker-compose exec postgres psql -U postgres -d trailmarks

# Frontend shell
docker-compose exec frontend sh
```

## Development Workflow

### Local Development with Docker

For development, you can use Docker Compose while still editing code locally:

1. **Backend Development**:
   ```bash
   # Rebuild backend after changes
   docker-compose up -d --build backend
   ```

2. **Frontend Development**:
   ```bash
   # Rebuild frontend after changes
   docker-compose up -d --build frontend
   ```

### Alternative: Hybrid Development

For faster iteration during development, you can run services locally and only use Docker for infrastructure services:

```bash
# Start only infrastructure services (database, keycloak, jaeger)
docker-compose up -d postgres keycloak jaeger nginx-otlp

# Run backend locally
cd backend/src
dotnet run

# Run frontend locally  
cd frontend
npm start
```

This approach provides faster rebuild times during development while still ensuring infrastructure services are consistent.

### Database Management

#### Reinitialize Database

```bash
# Stop and remove containers and volumes
docker-compose down -v

# Start fresh with database initialization
docker-compose up -d
```

#### Access PostgreSQL

```bash
docker-compose exec postgres psql -U postgres -d trailmarks
```

#### Backup Database

```bash
docker-compose exec postgres pg_dump -U postgres trailmarks > backup.sql
```

#### Restore Database

```bash
cat backup.sql | docker-compose exec -T postgres psql -U postgres -d trailmarks
```

## Configuration

### Environment Variables

You can override default settings by creating a `docker-compose.override.yml` file:

```yaml
services:
  postgres:
    environment:
      POSTGRES_PASSWORD: your_secure_password

  backend:
    environment:
      ConnectionStrings__DefaultConnection: "Host=postgres;Port=5432;Database=trailmarks;Username=postgres;Password=your_secure_password;SSL Mode=Disable;Timezone=Europe/Berlin"
```

### Custom Ports

To change the exposed ports, edit the `ports` section in `docker-compose.yml`:

```yaml
services:
  frontend:
    ports:
      - "3000:80"  # Changed from 4200:80

  backend:
    ports:
      - "5000:8080"  # Changed from 8080:8080
```

## Production Deployment

For production deployment, consider:

1. **Security**:
   - Change default PostgreSQL password
   - Use environment variables for sensitive data
   - Enable HTTPS with a reverse proxy (nginx, Traefik)
   - Review CORS settings in the backend

2. **Performance**:
   - Use Docker Compose resource limits
   - Configure PostgreSQL for production workload
   - Use a CDN for frontend static assets

3. **Monitoring**:
   - Add logging aggregation (ELK stack, Grafana Loki)
   - Add metrics collection (Prometheus)
   - Set up health checks

## Troubleshooting

### Docker Build Fails with Network Errors

If you encounter network errors during Docker build (especially with NuGet or npm):

1. **Use host network for build**:
   ```bash
   docker-compose build --build-arg BUILDKIT_INLINE_CACHE=1
   ```

2. **Check Docker network settings**:
   - Ensure Docker has internet access
   - Check firewall settings
   - Try restarting Docker daemon

3. **Build with increased timeout**:
   ```bash
   COMPOSE_HTTP_TIMEOUT=300 docker-compose up -d --build
   ```

### Frontend Cannot Connect to Backend

If the frontend shows API errors:

1. Check that all services are running:
   ```bash
   docker-compose ps
   ```

2. Check backend logs:
   ```bash
   docker-compose logs backend
   ```

3. Verify backend is accessible:
   ```bash
   curl http://localhost:8080/health
   ```

### Database Connection Issues

If the backend cannot connect to the database:

1. Check PostgreSQL logs:
   ```bash
   docker-compose logs postgres
   ```

2. Verify database is ready:
   ```bash
   docker-compose exec postgres pg_isready -U postgres
   ```

3. Check connection string in backend logs

### Port Conflicts

If ports 4200, 8080, or 8180 are already in use:

1. Stop the conflicting services
2. Or change ports in `docker-compose.yml`

### Keycloak Issues

If Keycloak fails to start or is not accessible:

1. **Check Keycloak logs**:
   ```bash
   docker-compose logs keycloak
   ```

2. **Verify Keycloak database exists**:
   ```bash
   docker-compose exec postgres psql -U postgres -l | grep keycloak
   ```

3. **Check Keycloak health**:
   ```bash
   curl http://localhost:8180/health/ready
   ```

4. **Access Keycloak Admin Console**:
   - URL: http://localhost:8180
   - Username: `admin`
   - Password: `admin`

5. **Verify realm import**:
   - Login to Keycloak Admin Console
   - Check if `trailmarks` realm exists
   - Verify clients `trailmarks-frontend` and `trailmarks-backend` are configured

6. **Common issues**:
   - **Keycloak database not created**: Ensure the init script ran successfully. Check postgres logs.
   - **Realm not imported**: Verify `keycloak/realm-export.json` exists and is mounted correctly.
   - **Port conflict**: If port 8180 is in use, change it in docker-compose.yml
   - **Slow startup**: Keycloak can take 60-90 seconds to start. Check health check status.

### Rebuild from Scratch

If you encounter persistent issues:

```bash
# Stop and remove everything
docker-compose down -v

# Remove all images
docker-compose down --rmi all

# Rebuild and start
docker-compose up -d --build
```

## Build Artifacts

All builds happen inside Docker containers:
- Backend: Built with .NET SDK 8.0
- Frontend: Built with Node.js 20
- No local installation of build tools required

## Network Architecture

Services communicate via a Docker bridge network (`trailmarks-network`):
- Frontend → Backend: via `http://backend:8080` (internal) or `http://localhost:8080` (from browser)
- Frontend → Keycloak: via `http://localhost:8180` (from browser)
- Backend → Database: via `postgres:5432`
- Backend → Keycloak: via `http://keycloak:8080` (internal, for token validation)
- Keycloak → Database: via `postgres:5432` (keycloak database)
- All services can resolve each other by service name

## Volume Management

### List Volumes

```bash
docker volume ls | grep trailmarks
```

### Inspect Volume

```bash
docker volume inspect trailmarks_postgres-data
```

### Remove Volume

```bash
docker volume rm trailmarks_postgres-data
```

## OpenTelemetry and Observability

The Trailmarks application includes OpenTelemetry instrumentation for distributed tracing and performance monitoring.

### Jaeger UI

Access the Jaeger UI to visualize traces:

**URL**: http://localhost:16686

### Features

- **Distributed Tracing**: Track requests across frontend, backend, and database
- **Performance Analysis**: Identify slow operations and bottlenecks
- **Service Dependencies**: Visualize how services interact
- **Error Tracking**: Find and diagnose errors in the request flow

### Using Jaeger

1. **View Services**: Select "trailmarks-frontend" or "TrailmarksApi" from the service dropdown
2. **Search Traces**: Use filters to find specific traces or operations
3. **Analyze Traces**: Click on a trace to see detailed timing information
4. **Service Graph**: View the "System Architecture" tab to see service dependencies

### Instrumented Components

**Backend (TrailmarksApi):**
- ASP.NET Core HTTP requests
- HttpClient calls
- Entity Framework Core database queries

**Frontend (trailmarks-frontend):**
- HTTP requests (Fetch API and XMLHttpRequest)
- Document load performance
- User interactions

### Configuration

The OpenTelemetry endpoint is configured via environment variables:

**Backend:**
```yaml
environment:
  OpenTelemetry__OtlpEndpoint: "http://jaeger:4318"
```

**Frontend:**
See `frontend/src/environments/environment.ts` for OTLP endpoint configuration.

### Troubleshooting

If traces are not appearing in Jaeger:

1. Check that Jaeger is running: `docker-compose ps jaeger`
2. View Jaeger logs: `docker-compose logs jaeger`
3. Verify backend can reach Jaeger: `docker-compose exec backend ping jaeger`
4. Check browser console for frontend tracing errors

## Keycloak Authentication

The Trailmarks application includes Keycloak for authentication and authorization. Keycloak is pre-configured with a `trailmarks` realm and two clients for the frontend and backend.

### Keycloak Admin Console

Access the Keycloak Admin Console to manage users, roles, and configuration:

**URL**: http://localhost:8180

**Credentials:**
- Username: `admin`
- Password: `admin`

### Trailmarks Realm

The `trailmarks` realm is automatically imported on first startup from `keycloak/realm-export.json`.

**Realm Configuration:**
- **Realm Name**: `trailmarks`
- **Supported Locales**: English (en), German (de)
- **Default Locale**: English
- **Registration**: Enabled (users can self-register)
- **Email Verification**: Disabled (for development)
- **Password Reset**: Enabled
- **Remember Me**: Enabled
- **Brute Force Protection**: Enabled

### Roles

The realm includes three predefined roles:

1. **user**: Regular user role (default for all users)
2. **moderator**: Moderator role for content management
3. **admin**: Administrator role with full access

### Clients

#### trailmarks-frontend (Public Client)

Configuration for the Angular frontend application:

- **Client ID**: `trailmarks-frontend`
- **Client Type**: Public (no client secret required)
- **Protocol**: OpenID Connect
- **Valid Redirect URIs**: 
  - `http://localhost:4200/*`
  - `http://localhost/*`
- **Web Origins**: 
  - `http://localhost:4200`
  - `http://localhost`
- **Authentication Flow**: Authorization Code with PKCE (S256)
- **Direct Access Grants**: Enabled

**Token Claims:**
- `preferred_username`: Username
- `email`: Email address
- `roles`: Array of realm roles

#### trailmarks-backend (Bearer-Only Client)

Configuration for the ASP.NET Core backend API:

- **Client ID**: `trailmarks-backend`
- **Client Type**: Bearer-only (validates tokens but doesn't authenticate users)
- **Protocol**: OpenID Connect
- **Access Token Lifespan**: 300 seconds (5 minutes)

### User Management

**Creating Users:**

1. Login to Keycloak Admin Console
2. Select `trailmarks` realm
3. Navigate to "Users" → "Add user"
4. Fill in user details (username, email, first name, last name)
5. Click "Save"
6. Go to "Credentials" tab and set password
7. Optionally assign roles in "Role Mappings" tab

**Assigning Roles:**

1. Select user in "Users" list
2. Go to "Role Mappings" tab
3. Select desired roles from "Available Roles"
4. Click "Add selected"

### OpenID Connect Endpoints

The Keycloak realm provides standard OIDC endpoints:

- **Discovery**: `http://localhost:8180/realms/trailmarks/.well-known/openid-configuration`
- **Authorization**: `http://localhost:8180/realms/trailmarks/protocol/openid-connect/auth`
- **Token**: `http://localhost:8180/realms/trailmarks/protocol/openid-connect/token`
- **Userinfo**: `http://localhost:8180/realms/trailmarks/protocol/openid-connect/userinfo`
- **Logout**: `http://localhost:8180/realms/trailmarks/protocol/openid-connect/logout`

### Configuration for Production

For production deployment, update the following:

1. **Change admin password**: Update `KEYCLOAK_ADMIN_PASSWORD` in docker-compose.yml
2. **Enable HTTPS**: Configure SSL/TLS certificates and set `KC_HOSTNAME_STRICT_HTTPS=true`
3. **Update redirect URIs**: Add production URLs to client configuration
4. **Update web origins**: Add production domains to CORS configuration
5. **Enable email verification**: Set up SMTP and enable email verification
6. **Review security settings**: Adjust session timeouts, token lifespans, brute force protection

### Database

Keycloak uses a separate PostgreSQL database (`keycloak`) in the same PostgreSQL instance as the application database. The database is automatically created on first startup via the initialization script.

**Connection Details:**
- **Database**: `keycloak`
- **Host**: `postgres:5432`
- **User**: `postgres`
- **Password**: `postgres`

## Additional Resources

- [Docker Documentation](https://docs.docker.com/)
- [Docker Compose Documentation](https://docs.docker.com/compose/)
- [PostgreSQL Docker Image](https://hub.docker.com/_/postgres)
- [.NET Docker Images](https://hub.docker.com/_/microsoft-dotnet)
- [nginx Docker Image](https://hub.docker.com/_/nginx)
- [Keycloak Documentation](https://www.keycloak.org/documentation)
- [Keycloak Docker Image](https://quay.io/repository/keycloak/keycloak)
- [Jaeger Documentation](https://www.jaegertracing.io/docs/)
- [OpenTelemetry Documentation](https://opentelemetry.io/docs/)
