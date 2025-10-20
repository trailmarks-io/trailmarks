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

# Build migrations project separately and copy its output
WORKDIR /app/src/TrailmarksApi.Migrations
RUN dotnet publish -c Release -o /app/migrations-publish
# Copy all DLLs from migrations publish to main publish directory
RUN cp /app/migrations-publish/*.dll /app/publish/ 2>/dev/null || true
RUN cp /app/migrations-publish/*.pdb /app/publish/ 2>/dev/null || true

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
