# Trailmarks Backend (C# ASP.NET Core)

This is the backend API for the Trailmarks application, providing endpoints to manage Wandersteine (hiking stones).

## Technology Stack

- **C# ASP.NET Core 8.0** - Web framework
- **Entity Framework Core** - ORM for database operations
- **PostgreSQL 16** - Database
- **Swagger/OpenAPI** - API documentation
- **CORS** - Cross-origin resource sharing support
- **Testcontainers** - PostgreSQL containers for testing

### Package Management

This project uses **Central Package Management** via `Directory.Packages.props` to manage all NuGet package versions in a single location. All package versions are defined centrally in `/backend/Directory.Packages.props`, ensuring consistency across all projects.

To update package versions, edit the `Directory.Packages.props` file rather than individual `.csproj` files.

## Features

- RESTful API endpoints for Wandersteine management
- Automatic database migrations and seeding
- OpenAPI/Swagger documentation
- CORS support for frontend communication
- PostgreSQL database with Entity Framework Core
- Comprehensive logging and error handling
- Unit tests with PostgreSQL Testcontainers

## API Endpoints

- `GET /api/wandersteine/recent` - Returns the 5 most recently added hiking stones
- `GET /api/wandersteine` - Returns all hiking stones
- `GET /health` - Service health check
- `GET /swagger` - Interactive API documentation

## Configuration

The application can be configured via `appsettings.json`:

- **ConnectionStrings.DefaultConnection**: PostgreSQL connection string (required)

Example configuration for **development**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=trailmarks;Username=postgres;Password=postgres;SSL Mode=Disable;Timezone=Europe/Berlin"
  }
}
```

> **⚠️ Security Notes:**
> - The example above uses default credentials for **development only**
> - For **production**, use environment variables for credentials
> - For **production**, enable SSL: Change `SSL Mode=Disable` to `SSL Mode=Require`
> - Never commit production credentials to source control

## Quick Start

### Prerequisites

- .NET 8.0 SDK
- PostgreSQL 16 (or use Docker to run PostgreSQL)
- Docker (optional, for running PostgreSQL in a container)

### Running with PostgreSQL

1. Set up PostgreSQL database:
   ```bash
   # Option 1: Use Docker Compose (recommended)
   docker-compose up -d postgres
   
   # Option 2: Install PostgreSQL locally
   # Follow instructions at https://www.postgresql.org/download/
   ```

2. Update connection string in `appsettings.json` if needed

3. Initialize the database with sample data:
   ```bash
   cd backend/src
   dotnet run -- -DbInit
   ```

4. Run the application:
   ```bash
   cd backend/src
   dotnet run
   ```

The application starts on `http://localhost:8080` with PostgreSQL as the database.

## Database Schema

The application automatically creates the database schema on startup. Sample data is seeded if the database is empty.

### Wanderstein Model

- `Id` (uint) - Primary key
- `Name` (string) - Name of the hiking stone
- `UniqueId` (string) - Unique identifier (e.g., WS-2024-001)
- `PreviewUrl` (string) - URL to preview image
- `Description` (string) - Description text
- `Location` (string) - Location information
- `CreatedAt` (DateTime) - Creation timestamp
- `UpdatedAt` (DateTime) - Last update timestamp

## API Response Format

All Wandersteine endpoints return data in the following format:

```json
{
  "id": 1,
  "name": "Schwarzwaldstein",
  "unique_id": "WS-2024-001",
  "preview_url": "https://picsum.photos/300/200?random=1",
  "created_at": "2025-08-04T12:00:00Z"
}
```

## Development

### Building

```bash
cd backend/src
dotnet build
```

### Running Tests

The backend has comprehensive xUnit tests covering all controllers, services, and models. Tests use PostgreSQL Testcontainers to ensure tests run against a real PostgreSQL database.

**Prerequisites:**
- Docker must be running (for Testcontainers)

```bash
cd backend/test
dotnet test
```

Test coverage includes:
- Controller tests (18 tests) - HealthController, WandersteineController, TranslationsController
- Service tests (5 tests) - DatabaseService
- Model tests (4 tests) - WandersteinResponse mapping
- OpenTelemetry tests (2 tests) - Configuration tests

**Note:** Tests automatically start a PostgreSQL container using Testcontainers. The first test run may take longer as Docker images are downloaded.

## Documentation

Interactive API documentation is available at `/swagger` when the application is running.