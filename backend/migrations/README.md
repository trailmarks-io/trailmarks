# TrailmarksApi.Migrations

This project contains all Entity Framework Core database migrations for the Trailmarks API.

## Overview

The migrations are managed separately from the main API project to provide a clear separation of concerns and make database updates easier to manage.

## Creating New Migrations

To create a new migration:

```bash
cd backend/migrations
dotnet ef migrations add <MigrationName>
```

Example:
```bash
dotnet ef migrations add AddUserTable
```

## Applying Migrations

Migrations are automatically applied when the API starts with the `-DbInit` flag, or you can use the provided script:

```bash
cd backend
./scripts/init-database.sh
```

## Migration Structure

- **InitialCreate**: Creates the initial database schema with Wandersteine and Translations tables
- **SeedData**: Seeds the database with initial test data

## Database Providers

The migrations support both:
- **SQLite** (for development and testing)
- **PostgreSQL** (for production)

The design-time factory uses SQLite by default for creating migrations, but the actual database provider is configured at runtime based on the application's configuration.

## Seed Data

Initial seed data is included in the `SeedData` migration to provide:
- 18 sample Wandersteine (hiking stones) across different locations
- Translations for both German (de) and English (en) languages

## Technical Details

- The migrations assembly is configured via the `MigrationsAssembly` option in the main API project
- The assembly is pre-loaded at runtime to ensure proper loading
- Migrations are applied using `Database.Migrate()` instead of the deprecated `EnsureCreated()`
