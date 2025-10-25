# Docker Deployment Guide

This guide explains how to deploy the Trailmarks application using Docker and Docker Compose.

## Prerequisites

- Docker Engine 20.10 or higher
- Docker Compose V2 or higher
- No local installation of .NET, Node.js, or PostgreSQL required

## Quick Start

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
- **Jaeger UI (Tracing)**: http://localhost:16686

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
- **Credentials**:
  - Database: `trailmarks`
  - User: `postgres`
  - Password: `postgres`

### Observability (Jaeger)
- **Container Name**: `trailmarks-jaeger`
- **Port**: 16686 (Jaeger UI), 4318 (internal OTLP receiver)
- **Technology**: Jaeger all-in-one with OpenTelemetry support
- **Purpose**: Distributed tracing and performance monitoring
- **Features**:
  - Trace visualization
  - Service dependency graph
  - Performance analysis
  - Request flow tracking

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

For faster iteration during development, you can run services locally and only use Docker for the database:

```bash
# Start only the database
docker-compose up -d postgres

# Run backend locally
cd backend/src
dotnet run

# Run frontend locally  
cd frontend
npm start
```

This approach provides faster rebuild times during development while still ensuring the database is consistent.

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

If ports 4200 or 8080 are already in use:

1. Stop the conflicting services
2. Or change ports in `docker-compose.yml`

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
- Backend → Database: via `postgres:5432`
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

## Additional Resources

- [Docker Documentation](https://docs.docker.com/)
- [Docker Compose Documentation](https://docs.docker.com/compose/)
- [PostgreSQL Docker Image](https://hub.docker.com/_/postgres)
- [.NET Docker Images](https://hub.docker.com/_/microsoft-dotnet)
- [nginx Docker Image](https://hub.docker.com/_/nginx)
- [Jaeger Documentation](https://www.jaegertracing.io/docs/)
- [OpenTelemetry Documentation](https://opentelemetry.io/docs/)
