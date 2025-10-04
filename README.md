# Trailmarks - Hiking Stones Overview

A web application for displaying the most recently added hiking stones.

## Architecture

### Backend
- **Framework**: C# ASP.NET Core 8.0
- **Database**: PostgreSQL with Entity Framework Core (SQLite fallback for development)
- **API Documentation**: OpenAPI (Swagger)
- **Features**: 
  - REST API for hiking stones
  - Automatic database migrations
  - Sample data for development
  - CORS support
  - SQLite fallback for local development

### Frontend
- **Framework**: Angular 20.1.0
- **Styling**: Tailwind CSS v3 (Utility-First CSS Framework)
- **HTTP Client**: Angular HttpClient for API communication
- **Internationalization**: Multi-language support (German/English)
- **Features**:
  - Overview page of the 5 most recent hiking stones
  - Responsive design for mobile devices
  - Error handling and loading status
  - Language switcher with persistence
  - Runtime-editable translations (no redeployment needed)

## API Endpoints

- `GET /api/wandersteine/recent` - The 5 most recently added hiking stones
- `GET /api/wandersteine` - All hiking stones
- `GET /health` - Health Check
- `GET /swagger` - API documentation (interactive Swagger UI)

## Installation and Startup

### Prerequisites
- .NET 8.0 SDK
- Node.js 20+
- PostgreSQL (optional, SQLite is automatically used for development)

### Backend
```bash
cd backend
dotnet run
```

The backend server runs on port 8080. On first startup, a SQLite database is automatically created and populated with sample data.

To initialize the database with sample data:
```bash
cd backend
dotnet run -- -DbInit
```

### Frontend
```bash
cd frontend
npm install
npx ng serve
```

The frontend server runs on port 4200. Alternatively, you can use `npm start`.

## Configuration

The backend can be configured via `appsettings.json` or `appsettings.Development.json`:

### Development (SQLite)
SQLite is used by default for local development:
```json
{
  "UseSqlite": true
}
```

### Production (PostgreSQL)
For using PostgreSQL:
```json
{
  "UseSqlite": false,
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=trailmarks;Username=postgres;Password=yourpassword"
  }
}
```

## Data Model

### Wanderstein Entity
The complete data model in the database:
- `Id` (uint) - Primary key
- `Name` (string, max 200) - Name of the hiking stone
- `UniqueId` (string, max 50) - Unique identifier (e.g., WS-2024-001)
- `PreviewUrl` (string, max 500) - URL to preview image
- `Description` (string, max 1000) - Description text
- `Location` (string, max 200) - Location information
- `CreatedAt` (DateTime) - Creation timestamp
- `UpdatedAt` (DateTime) - Last update timestamp

### API Response Format
The API endpoints return a simplified version:
```json
{
  "id": 1,
  "name": "Schwarzwaldstein",
  "unique_id": "WS-2024-001",
  "preview_url": "https://picsum.photos/300/200?random=1",
  "created_at": "2025-08-04T12:00:00Z"
}
```

## Features

✅ REST API with C# ASP.NET Core 8.0  
✅ PostgreSQL database integration with Entity Framework Core  
✅ SQLite fallback for local development  
✅ OpenAPI/Swagger documentation  
✅ Angular 20.1.0 Frontend  
✅ Responsive Design  
✅ Multi-language support (German/English)  
✅ Automatic database migrations  
✅ Sample data for development  
✅ CORS support  
✅ Comprehensive error handling and logging  

## Development

### Backend Tests
```bash
cd backend
dotnet test
```

### Backend Build
```bash
cd backend
dotnet build
```

### Frontend Tests
```bash
cd frontend
npx ng test
```

### Frontend Build
```bash
cd frontend
npx ng build
```

Alternatively, Angular CLI can be installed globally:
```bash
npm install -g @angular/cli
ng build
```

### API Documentation
The interactive API documentation is available at: http://localhost:8080/swagger

## Internationalization (i18n)

The application supports multiple languages with runtime-editable translations.

### Supported Languages
- 🇩🇪 German (Deutsch) - Default
- 🇬🇧 English

### Language Switching
Users can switch languages using the language switcher in the top-right corner of the application. The selected language is persisted in the browser's localStorage and automatically restored on the next visit.

### Adding/Editing Translations

Translation files are located in `/frontend/public/assets/i18n/`:
- `de.json` - German translations
- `en.json` - English translations

To modify translations:
1. Edit the appropriate JSON file
2. Changes take effect immediately without rebuilding or redeploying the application
3. Simply refresh the browser to see the changes

Example translation file structure:
```json
{
  "common": {
    "loading": "Loading...",
    "error": "Error",
    "retry": "Retry"
  },
  "wanderstein": {
    "title": "Latest Hiking Stones",
    "subtitle": "The 5 most recently added hiking stones",
    "addedOn": "Added on"
  }
}
```

### Adding New Languages

To add a new language:
1. Create a new JSON file in `/frontend/public/assets/i18n/` (e.g., `fr.json` for French)
2. Add the language code to `SupportedLanguage` type in `/frontend/src/app/services/language.ts`
3. Update the `getSupportedLanguages()` method to include the new language
4. Update the `getLanguageLabel()` method in the language switcher component
