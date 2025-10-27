# Trailmarks - Hiking Stones Overview

![.NET 8.0](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![Angular 20.1](https://img.shields.io/badge/Angular-20.1-DD0031?logo=angular)
![PostgreSQL 16](https://img.shields.io/badge/PostgreSQL-16-336791?logo=postgresql)
![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?logo=docker)

A modern web application for displaying and managing hiking stones (Wandersteine).

## 🚀 Quick Start

### With Docker (Recommended)

```bash
git clone https://github.com/trailmarks-io/trailmarks.git
cd trailmarks
docker-compose up -d
```

**Access the application:**
- 🌐 Frontend: http://localhost:4200
- 🔌 Backend API: http://localhost:8080
- 📚 API Documentation: http://localhost:8080/swagger
- 🔐 Keycloak Admin: http://localhost:8180
- 📊 Tracing UI: http://localhost:16686

### Local Development

**Backend:**
```bash
cd backend
dotnet run
```

**Frontend:**
```bash
cd frontend
npm install
npx ng serve
```

## ✨ Features

✅ REST API with C# ASP.NET Core 8.0  
✅ PostgreSQL database with Entity Framework Core  
✅ Angular 20.1.0 frontend with Tailwind CSS  
✅ Multi-language support (German/English)  
✅ Keycloak authentication and authorization  
✅ Docker deployment with Docker Compose  
✅ OpenTelemetry instrumentation with Jaeger tracing  
✅ Responsive design for mobile devices  
✅ Comprehensive testing (xUnit, Jasmine/Karma, Playwright)

## 📚 Documentation

Comprehensive documentation is available in the **[docs/](docs/)** folder:

- **[Architecture Documentation](docs/architecture/index.adoc)** - Technical architecture following ARC42 template
- **[User Guide](docs/user-guide/index.adoc)** - End user documentation
- **[Admin Guide](docs/admin-guide/index.adoc)** - System administration and content management

> **Note:** The documentation is written in AsciiDoc format and can be converted to HTML/Markdown for viewing.

## 🛠️ Technology Stack

- **Backend**: .NET 8.0, ASP.NET Core, Entity Framework Core
- **Frontend**: Angular 20.1.0, TypeScript, Tailwind CSS
- **Database**: PostgreSQL 16 with PostGIS
- **Authentication**: Keycloak 26.0.7 (OpenID Connect / OAuth2)
- **API Documentation**: OpenAPI 3.0 / Swagger
- **Observability**: OpenTelemetry, Jaeger
- **Containerization**: Docker, Docker Compose

## 📁 Project Structure

```
trailmarks/
├── backend/              # .NET 8.0 Backend API
│   ├── src/             # Application source code
│   └── test/            # xUnit tests
├── frontend/            # Angular 20.1 Frontend
│   ├── src/             # Application source code
│   ├── e2e/             # Playwright E2E tests
│   └── public/          # Static assets
├── docs/                # Documentation (AsciiDoc)
│   ├── architecture/    # ARC42 architecture docs
│   ├── user-guide/      # End user guide
│   └── admin-guide/     # Admin & moderator guide
└── docker-compose.yml   # Docker deployment
```

## 🧪 Testing

```bash
# Backend tests
cd backend && dotnet test

# Frontend unit tests
cd frontend && npx ng test

# Frontend E2E tests
cd frontend && npm run e2e
```

## 🔨 Building

```bash
# Backend
cd backend && dotnet build

# Frontend
cd frontend && npx ng build
```

## 🤝 Contributing

Contributions are welcome! Please see the [architecture documentation](docs/architecture/index.adoc) for technical details.

### Git Workflow

- **Main branch**: `main` - stable production code
- **Development branch**: `develop` - active development
- **Feature branches**: `<issue-number>-description`
- **Commit convention**: Conventional Commits

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 💬 Support

- **Documentation**: [Full Documentation](docs/index.adoc)
- **Issues**: https://github.com/trailmarks-io/trailmarks/issues
- **API Documentation**: http://localhost:8080/swagger
