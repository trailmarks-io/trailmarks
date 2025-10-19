using Microsoft.EntityFrameworkCore;
using TrailmarksApi.Data;
using TrailmarksApi.Models;

namespace TrailmarksApi.Services
{
    /// <summary>
    /// Service for database initialization and seeding
    /// </summary>
    public class DatabaseService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DatabaseService> _logger;

        public DatabaseService(ApplicationDbContext context, ILogger<DatabaseService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Initialize the database with migrations and seed data
        /// </summary>
        public async Task InitializeAsync()
        {
            try
            {
                // Ensure database is created and migrations are applied
                await _context.Database.EnsureCreatedAsync();
                
                // Seed data if the database is empty
                await SeedDataAsync();
                
                _logger.LogInformation("Database initialized successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initializing the database");
                throw;
            }
        }

        /// <summary>
        /// Seed the database with sample Wandersteine if empty
        /// </summary>
        private async Task SeedDataAsync()
        {
            await SeedWandersteineAsync();
            await SeedTranslationsAsync();
        }

        /// <summary>
        /// Seed Wandersteine data
        /// </summary>
        private async Task SeedWandersteineAsync()
        {
            // Check if data already exists
            if (await _context.Wandersteine.AnyAsync())
            {
                _logger.LogInformation("Database already contains Wandersteine data, skipping seed");
                return;
            }

            _logger.LogInformation("Seeding database with sample Wandersteine");

            var sampleStones = new List<Wanderstein>
            {
                new Wanderstein
                {
                    Name = "Schwarzwaldstein",
                    UniqueId = "WS-2024-001",
                    PreviewUrl = "https://picsum.photos/300/200?random=1",
                    Description = "Ein historischer Wanderstein im Herzen des Schwarzwaldes",
                    Location = "Schwarzwald, Baden-Württemberg",
                    Latitude = 48.3019,
                    Longitude = 8.2392,
                    CreatedAt = DateTime.UtcNow.AddDays(-6),
                    UpdatedAt = DateTime.UtcNow.AddDays(-6)
                },
                new Wanderstein
                {
                    Name = "Rocky Mountain Summit",
                    UniqueId = "WS-2024-002",
                    PreviewUrl = "https://picsum.photos/300/200?random=2",
                    Description = "Wanderstein mit herrlichem Blick auf die Rocky Mountains",
                    Location = "Colorado, USA",
                    Latitude = 39.7392,
                    Longitude = -104.9903,
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    UpdatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new Wanderstein
                {
                    Name = "Mount Fuji Trail",
                    UniqueId = "WS-2024-003",
                    PreviewUrl = "https://picsum.photos/300/200?random=3",
                    Description = "Markanter Stein auf dem Weg zum Mount Fuji",
                    Location = "Fujinomiya, Japan",
                    Latitude = 35.3606,
                    Longitude = 138.7278,
                    CreatedAt = DateTime.UtcNow.AddDays(-4),
                    UpdatedAt = DateTime.UtcNow.AddDays(-4)
                },
                new Wanderstein
                {
                    Name = "Alpenblick",
                    UniqueId = "WS-2024-004",
                    PreviewUrl = "https://picsum.photos/300/200?random=4",
                    Description = "Wanderstein auf dem höchsten Punkt der Route",
                    Location = "Allgäu, Bayern",
                    Latitude = 47.5596,
                    Longitude = 10.7498,
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    UpdatedAt = DateTime.UtcNow.AddDays(-3)
                },
                new Wanderstein
                {
                    Name = "Outback Stone",
                    UniqueId = "WS-2024-005",
                    PreviewUrl = "https://picsum.photos/300/200?random=5",
                    Description = "Ruhiger Wanderstein im australischen Outback",
                    Location = "Uluru, Northern Territory, Australia",
                    Latitude = -25.3444,
                    Longitude = 131.0369,
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    UpdatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new Wanderstein
                {
                    Name = "Patagonia Vista",
                    UniqueId = "WS-2024-006",
                    PreviewUrl = "https://picsum.photos/300/200?random=6",
                    Description = "Wanderstein mit Blick auf die patagonische Landschaft",
                    Location = "Torres del Paine, Chile",
                    Latitude = -51.2527,
                    Longitude = -72.9653,
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    UpdatedAt = DateTime.UtcNow.AddDays(-1)
                }
            };

            _context.Wandersteine.AddRange(sampleStones);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Successfully seeded {sampleStones.Count} Wandersteine");
        }

        /// <summary>
        /// Seed translation data
        /// </summary>
        private async Task SeedTranslationsAsync()
        {
            // Check if translations already exist
            if (await _context.Translations.AnyAsync())
            {
                _logger.LogInformation("Database already contains translation data, skipping seed");
                return;
            }

            _logger.LogInformation("Seeding database with translations");

            var translations = new List<Translation>
            {
                // German translations
                new Translation { Key = "common.loading", Language = "de", Value = "Lädt..." },
                new Translation { Key = "common.error", Language = "de", Value = "Fehler" },
                new Translation { Key = "common.retry", Language = "de", Value = "Erneut versuchen" },
                new Translation { Key = "common.noData", Language = "de", Value = "Keine Daten gefunden" },
                new Translation { Key = "header.language", Language = "de", Value = "Sprache" },
                new Translation { Key = "wanderstein.title", Language = "de", Value = "Neueste Wandersteine" },
                new Translation { Key = "wanderstein.subtitle", Language = "de", Value = "Die 5 zuletzt hinzugefügten Wandersteine" },
                new Translation { Key = "wanderstein.loading", Language = "de", Value = "Lade Wandersteine..." },
                new Translation { Key = "wanderstein.error", Language = "de", Value = "Fehler beim Laden der Wandersteine" },
                new Translation { Key = "wanderstein.noData", Language = "de", Value = "Keine Wandersteine gefunden." },
                new Translation { Key = "wanderstein.addedOn", Language = "de", Value = "Hinzugefügt" },
                new Translation { Key = "wanderstein.map.title", Language = "de", Value = "Kartenübersicht" },
                new Translation { Key = "wanderstein.recent.title", Language = "de", Value = "Neueste Wandersteine" },

                // English translations
                new Translation { Key = "common.loading", Language = "en", Value = "Loading..." },
                new Translation { Key = "common.error", Language = "en", Value = "Error" },
                new Translation { Key = "common.retry", Language = "en", Value = "Retry" },
                new Translation { Key = "common.noData", Language = "en", Value = "No data found" },
                new Translation { Key = "header.language", Language = "en", Value = "Language" },
                new Translation { Key = "wanderstein.title", Language = "en", Value = "Latest Hiking Stones" },
                new Translation { Key = "wanderstein.subtitle", Language = "en", Value = "The 5 most recently added hiking stones" },
                new Translation { Key = "wanderstein.loading", Language = "en", Value = "Loading hiking stones..." },
                new Translation { Key = "wanderstein.error", Language = "en", Value = "Error loading hiking stones" },
                new Translation { Key = "wanderstein.noData", Language = "en", Value = "No hiking stones found." },
                new Translation { Key = "wanderstein.addedOn", Language = "en", Value = "Added on" },
                new Translation { Key = "wanderstein.map.title", Language = "en", Value = "Map Overview" },
                new Translation { Key = "wanderstein.recent.title", Language = "en", Value = "Recent Hiking Stones" }
            };

            _context.Translations.AddRange(translations);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Successfully seeded {translations.Count} translations");
        }
    }
}