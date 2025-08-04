# Trailmarks Backend (C# ASP.NET Core)

This is the backend API for the Trailmarks application, providing endpoints to manage Wandersteine (hiking stones).

## Technology Stack

- **C# ASP.NET Core 8.0** - Web framework
- **Entity Framework Core** - ORM for database operations
- **PostgreSQL** - Primary database (with SQLite fallback for development)
- **Swagger/OpenAPI** - API documentation
- **CORS** - Cross-origin resource sharing support

## Features

- RESTful API endpoints for Wandersteine management
- Automatic database migrations and seeding
- OpenAPI/Swagger documentation
- CORS support for frontend communication
- PostgreSQL support with SQLite fallback for development
- Comprehensive logging and error handling

## API Endpoints

- `GET /api/wandersteine/recent` - Returns the 5 most recently added hiking stones
- `GET /api/wandersteine` - Returns all hiking stones
- `GET /health` - Service health check
- `GET /swagger` - Interactive API documentation

## Configuration

The application can be configured via `appsettings.json`:

- **UseSqlite**: Set to `true` to use SQLite instead of PostgreSQL
- **ConnectionStrings.DefaultConnection**: PostgreSQL connection string

## Quick Start

### Prerequisites

- .NET 8.0 SDK
- PostgreSQL (optional, SQLite fallback available)

### Running with SQLite (Development)

```bash
cd backend
dotnet run
```

The application starts on `http://localhost:8080` and uses SQLite by default for development.

### Running with PostgreSQL

1. Set up PostgreSQL database
2. Update connection string in `appsettings.json`
3. Set `"UseSqlite": false` in `appsettings.json`
4. Run the application:

```bash
cd backend
dotnet run
```

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
dotnet build
```

### Running Tests

```bash
dotnet test
```

## Documentation

Interactive API documentation is available at `/swagger` when the application is running.