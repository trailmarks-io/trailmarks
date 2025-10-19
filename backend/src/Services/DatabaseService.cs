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
                // Germany - Black Forest cluster
                new Wanderstein
                {
                    Name = "Schwarzwaldstein",
                    UniqueId = "WS-2024-001",
                    PreviewUrl = "https://picsum.photos/300/200?random=1",
                    Description = "Ein historischer Wanderstein im Herzen des Schwarzwaldes",
                    Location = "Schwarzwald, Baden-Württemberg",
                    Coordinates = new GeoCoordinate(48.3019, 8.2392),
                    CreatedAt = DateTime.UtcNow.AddDays(-20),
                    UpdatedAt = DateTime.UtcNow.AddDays(-20)
                },
                new Wanderstein
                {
                    Name = "Feldbergblick",
                    UniqueId = "WS-2024-007",
                    PreviewUrl = "https://picsum.photos/300/200?random=7",
                    Description = "Wanderstein am höchsten Punkt des Schwarzwaldes",
                    Location = "Feldberg, Baden-Württemberg",
                    Coordinates = new GeoCoordinate(47.8742, 8.0044),
                    CreatedAt = DateTime.UtcNow.AddDays(-19),
                    UpdatedAt = DateTime.UtcNow.AddDays(-19)
                },
                new Wanderstein
                {
                    Name = "Titisee Rundweg",
                    UniqueId = "WS-2024-008",
                    PreviewUrl = "https://picsum.photos/300/200?random=8",
                    Description = "Malerischer Wanderstein am Titisee",
                    Location = "Titisee-Neustadt, Baden-Württemberg",
                    Coordinates = new GeoCoordinate(47.9034, 8.2064),
                    CreatedAt = DateTime.UtcNow.AddDays(-18),
                    UpdatedAt = DateTime.UtcNow.AddDays(-18)
                },
                new Wanderstein
                {
                    Name = "Triberger Wasserfall",
                    UniqueId = "WS-2024-009",
                    PreviewUrl = "https://picsum.photos/300/200?random=9",
                    Description = "Wanderstein bei Deutschlands höchsten Wasserfällen",
                    Location = "Triberg, Baden-Württemberg",
                    Coordinates = new GeoCoordinate(48.1294, 8.2303),
                    CreatedAt = DateTime.UtcNow.AddDays(-17),
                    UpdatedAt = DateTime.UtcNow.AddDays(-17)
                },
                
                // Germany - Rhine Valley cluster
                new Wanderstein
                {
                    Name = "Loreley Felsen",
                    UniqueId = "WS-2024-010",
                    PreviewUrl = "https://picsum.photos/300/200?random=10",
                    Description = "Legendärer Wanderstein am Rhein",
                    Location = "St. Goarshausen, Rheinland-Pfalz",
                    Coordinates = new GeoCoordinate(50.1389, 7.7311),
                    CreatedAt = DateTime.UtcNow.AddDays(-16),
                    UpdatedAt = DateTime.UtcNow.AddDays(-16)
                },
                new Wanderstein
                {
                    Name = "Burg Rheinfels",
                    UniqueId = "WS-2024-011",
                    PreviewUrl = "https://picsum.photos/300/200?random=11",
                    Description = "Wanderstein an historischer Burgruine",
                    Location = "St. Goar, Rheinland-Pfalz",
                    Coordinates = new GeoCoordinate(50.1503, 7.7142),
                    CreatedAt = DateTime.UtcNow.AddDays(-15),
                    UpdatedAt = DateTime.UtcNow.AddDays(-15)
                },
                new Wanderstein
                {
                    Name = "Rheinsteig Aussicht",
                    UniqueId = "WS-2024-012",
                    PreviewUrl = "https://picsum.photos/300/200?random=12",
                    Description = "Panoramablick über das Rheintal",
                    Location = "Boppard, Rheinland-Pfalz",
                    Coordinates = new GeoCoordinate(50.2319, 7.5897),
                    CreatedAt = DateTime.UtcNow.AddDays(-14),
                    UpdatedAt = DateTime.UtcNow.AddDays(-14)
                },
                
                // Germany - Bavarian Alps cluster
                new Wanderstein
                {
                    Name = "Alpenblick",
                    UniqueId = "WS-2024-004",
                    PreviewUrl = "https://picsum.photos/300/200?random=4",
                    Description = "Wanderstein auf dem höchsten Punkt der Route",
                    Location = "Allgäu, Bayern",
                    Coordinates = new GeoCoordinate(47.5596, 10.7498),
                    CreatedAt = DateTime.UtcNow.AddDays(-13),
                    UpdatedAt = DateTime.UtcNow.AddDays(-13)
                },
                new Wanderstein
                {
                    Name = "Nebelhorn",
                    UniqueId = "WS-2024-013",
                    PreviewUrl = "https://picsum.photos/300/200?random=13",
                    Description = "Hochalpiner Wanderstein mit 400-Gipfel-Blick",
                    Location = "Oberstdorf, Bayern",
                    Coordinates = new GeoCoordinate(47.4119, 10.3233),
                    CreatedAt = DateTime.UtcNow.AddDays(-12),
                    UpdatedAt = DateTime.UtcNow.AddDays(-12)
                },
                new Wanderstein
                {
                    Name = "Königssee Panorama",
                    UniqueId = "WS-2024-014",
                    PreviewUrl = "https://picsum.photos/300/200?random=14",
                    Description = "Wanderstein am smaragdgrünen Königssee",
                    Location = "Schönau am Königssee, Bayern",
                    Coordinates = new GeoCoordinate(47.5667, 12.9833),
                    CreatedAt = DateTime.UtcNow.AddDays(-11),
                    UpdatedAt = DateTime.UtcNow.AddDays(-11)
                },
                new Wanderstein
                {
                    Name = "Watzmann Ostwand",
                    UniqueId = "WS-2024-015",
                    PreviewUrl = "https://picsum.photos/300/200?random=15",
                    Description = "Wanderstein mit Blick auf die berühmte Ostwand",
                    Location = "Berchtesgaden, Bayern",
                    Coordinates = new GeoCoordinate(47.5550, 12.9350),
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    UpdatedAt = DateTime.UtcNow.AddDays(-10)
                },
                
                // Germany - Harz cluster
                new Wanderstein
                {
                    Name = "Brocken Gipfel",
                    UniqueId = "WS-2024-016",
                    PreviewUrl = "https://picsum.photos/300/200?random=16",
                    Description = "Wanderstein auf dem höchsten Harzgipfel",
                    Location = "Wernigerode, Sachsen-Anhalt",
                    Coordinates = new GeoCoordinate(51.7992, 10.6147),
                    CreatedAt = DateTime.UtcNow.AddDays(-9),
                    UpdatedAt = DateTime.UtcNow.AddDays(-9)
                },
                new Wanderstein
                {
                    Name = "Hexentanzplatz",
                    UniqueId = "WS-2024-017",
                    PreviewUrl = "https://picsum.photos/300/200?random=17",
                    Description = "Mystischer Wanderstein an sagenhaftem Ort",
                    Location = "Thale, Sachsen-Anhalt",
                    Coordinates = new GeoCoordinate(51.7503, 11.0308),
                    CreatedAt = DateTime.UtcNow.AddDays(-8),
                    UpdatedAt = DateTime.UtcNow.AddDays(-8)
                },
                new Wanderstein
                {
                    Name = "Rappbodetalsperre",
                    UniqueId = "WS-2024-018",
                    PreviewUrl = "https://picsum.photos/300/200?random=18",
                    Description = "Wanderstein an der größten Talsperre im Harz",
                    Location = "Oberharz am Brocken, Sachsen-Anhalt",
                    Coordinates = new GeoCoordinate(51.7489, 10.9044),
                    CreatedAt = DateTime.UtcNow.AddDays(-7),
                    UpdatedAt = DateTime.UtcNow.AddDays(-7)
                },
                
                // International stones (keeping some for worldwide distribution)
                new Wanderstein
                {
                    Name = "Rocky Mountain Summit",
                    UniqueId = "WS-2024-002",
                    PreviewUrl = "https://picsum.photos/300/200?random=2",
                    Description = "Wanderstein mit herrlichem Blick auf die Rocky Mountains",
                    Location = "Colorado, USA",
                    Coordinates = new GeoCoordinate(39.7392, -104.9903),
                    CreatedAt = DateTime.UtcNow.AddDays(-6),
                    UpdatedAt = DateTime.UtcNow.AddDays(-6)
                },
                new Wanderstein
                {
                    Name = "Mount Fuji Trail",
                    UniqueId = "WS-2024-003",
                    PreviewUrl = "https://picsum.photos/300/200?random=3",
                    Description = "Markanter Stein auf dem Weg zum Mount Fuji",
                    Location = "Fujinomiya, Japan",
                    Coordinates = new GeoCoordinate(35.3606, 138.7278),
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    UpdatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new Wanderstein
                {
                    Name = "Outback Stone",
                    UniqueId = "WS-2024-005",
                    PreviewUrl = "https://picsum.photos/300/200?random=5",
                    Description = "Ruhiger Wanderstein im australischen Outback",
                    Location = "Uluru, Northern Territory, Australia",
                    Coordinates = new GeoCoordinate(-25.3444, 131.0369),
                    CreatedAt = DateTime.UtcNow.AddDays(-4),
                    UpdatedAt = DateTime.UtcNow.AddDays(-4)
                },
                new Wanderstein
                {
                    Name = "Patagonia Vista",
                    UniqueId = "WS-2024-006",
                    PreviewUrl = "https://picsum.photos/300/200?random=6",
                    Description = "Wanderstein mit Blick auf die patagonische Landschaft",
                    Location = "Torres del Paine, Chile",
                    Coordinates = new GeoCoordinate(-51.2527, -72.9653),
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    UpdatedAt = DateTime.UtcNow.AddDays(-3)
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