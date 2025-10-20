# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj files and restore dependencies
COPY src/TrailmarksApi/*.csproj ./src/TrailmarksApi/
COPY src/TrailmarksApi.Migrations/*.csproj ./src/TrailmarksApi.Migrations/
RUN dotnet restore ./src/TrailmarksApi/TrailmarksApi.csproj

# Copy everything else and build
COPY src/ ./src/
WORKDIR /app/src/TrailmarksApi
RUN dotnet publish -c Release -o /app/publish

# Build migrations project and copy its DLL
WORKDIR /app/src/TrailmarksApi.Migrations
RUN dotnet build -c Release -o /app/migrations-build
RUN ls -la /app/migrations-build/
RUN cp -v /app/migrations-build/TrailmarksApi.Migrations.dll /app/publish/
RUN cp -v /app/migrations-build/TrailmarksApi.Migrations.pdb /app/publish/ || true
RUN ls -la /app/publish/ | grep Migrations

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published application from build stage
COPY --from=build /app/publish .

# Expose port
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Run the application
ENTRYPOINT ["dotnet", "TrailmarksApi.dll"]
