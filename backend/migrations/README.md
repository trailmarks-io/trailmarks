# EF Core Migrations Project

This project contains all database migrations for the Trailmarks application.

## Structure

```
TrailmarksApi.Migrations/
├── Migrations/
│   ├── 20251020040222_InitialCreate.cs          # Initial database schema
│   ├── 20251020040254_SeedInitialData.cs        # Seed data for Wandersteine and Translations
│   └── ApplicationDbContextModelSnapshot.cs      # Current model snapshot
├── ApplicationDbContextFactory.cs                 # Design-time DbContext factory
├── Properties/
│   └── AssemblyInfo.cs                           # Assembly configuration
└── TrailmarksApi.Migrations.csproj               # Project file
```

## Design Decisions

### Separate Migrations Project

Migrations are managed in a separate project to:
- Clearly separate database schema management from application code
- Allow independent versioning of migrations
- Enable reuse across multiple projects if needed

### File Linking Strategy

Instead of referencing the main API project (which would create a circular dependency), this project uses file links to reference the DbContext and Model files:

```xml
<Compile Include="..\src\Data\ApplicationDbContext.cs" Link="Data\ApplicationDbContext.cs" />
<Compile Include="..\src\Models\Wanderstein.cs" Link="Models\Wanderstein.cs" />
<Compile Include="..\src\Models\GeoCoordinate.cs" Link="Models\GeoCoordinate.cs" />
<Compile Include="..\src\Models\Translation.cs" Link="Models\Translation.cs" />
```

This approach:
- Avoids circular dependencies
- Keeps the source files in the main project
- Allows the migrations project to compile independently

### Runtime Assembly Loading

The main API project preloads the migrations assembly at startup:

```csharp
var migrationsAssemblyPath = Path.Combine(AppContext.BaseDirectory, "TrailmarksApi.Migrations.dll");
if (File.Exists(migrationsAssemblyPath))
{
    AssemblyLoadContext.Default.LoadFromAssemblyPath(migrationsAssemblyPath);
}
```

This ensures EF Core can discover and apply migrations at runtime.

## Creating New Migrations

To create a new migration:

```bash
cd backend/migrations
dotnet ef migrations add MigrationName
```

## Applying Migrations

### Development

Use the `-DbInit` flag when starting the application:

```bash
cd backend/src
dotnet run -- -DbInit
```

This will:
1. Create the database if it doesn't exist
2. Apply all pending migrations
3. Seed data if this is the first run

### Production

Migrations can be applied using the same `-DbInit` flag or programmatically through the DatabaseService.

## Seed Data

Initial seed data is included in the `SeedInitialData` migration and includes:
- 18 Wandersteine (hiking stones) from various locations
- 26 translations for German and English languages

## Architecture Notes

### Challenges with Separate Migrations Projects

Having migrations in a truly separate project introduces some complexity:

1. **Circular Dependencies**: The migrations project needs access to the DbContext and Models, but the main project needs the migrations assembly at runtime.

2. **Assembly Loading**: EF Core needs to discover the migrations assembly at runtime, which requires explicit loading or proper reference configuration.

3. **Test Environment**: Test projects need special configuration to ensure the migrations assembly is available and discoverable.

### Alternative Approach

If the complexity becomes problematic, consider moving migrations back to the main API project but keeping them in a dedicated `Migrations/` folder. This is the standard Microsoft-recommended approach and would simplify:
- Build configuration
- Assembly discovery
- Test environment setup

While keeping them in the main project, you can still achieve good separation by:
- Using a dedicated namespace (`TrailmarksApi.Migrations`)
- Keeping all migration files in a `/Migrations` folder
- Clearly documenting the migration management process

## Migration History

| Migration | Date | Description |
|-----------|------|-------------|
| InitialCreate | 2025-10-20 | Initial database schema with Wandersteine and Translations tables |
| SeedInitialData | 2025-10-20 | Seed data for 18 Wandersteine and 26 translations |
