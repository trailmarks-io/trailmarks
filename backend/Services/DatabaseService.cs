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
            // Check if data already exists
            if (await _context.Wandersteine.AnyAsync())
            {
                _logger.LogInformation("Database already contains data, skipping seed");
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
                    Description = "A historic hiking stone in the heart of the Black Forest",
                    Location = "Black Forest, Baden-Württemberg",
                    CreatedAt = DateTime.UtcNow.AddDays(-6),
                    UpdatedAt = DateTime.UtcNow.AddDays(-6)
                },
                new Wanderstein
                {
                    Name = "Alpenblick",
                    UniqueId = "WS-2024-002",
                    PreviewUrl = "https://picsum.photos/300/200?random=2",
                    Description = "Hiking stone with magnificent view of the Alps",
                    Location = "Allgäu, Bavaria",
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    UpdatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new Wanderstein
                {
                    Name = "Rheintalweg",
                    UniqueId = "WS-2024-003",
                    PreviewUrl = "https://picsum.photos/300/200?random=3",
                    Description = "Distinctive stone on the Rhine Valley Trail",
                    Location = "Rhine Valley, Baden-Württemberg",
                    CreatedAt = DateTime.UtcNow.AddDays(-4),
                    UpdatedAt = DateTime.UtcNow.AddDays(-4)
                },
                new Wanderstein
                {
                    Name = "Berggipfel",
                    UniqueId = "WS-2024-004",
                    PreviewUrl = "https://picsum.photos/300/200?random=4",
                    Description = "Hiking stone at the highest point of the route",
                    Location = "Harz, Lower Saxony",
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    UpdatedAt = DateTime.UtcNow.AddDays(-3)
                },
                new Wanderstein
                {
                    Name = "Waldlichtung",
                    UniqueId = "WS-2024-005",
                    PreviewUrl = "https://picsum.photos/300/200?random=5",
                    Description = "Peaceful hiking stone in a beautiful forest clearing",
                    Location = "Eifel, North Rhine-Westphalia",
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    UpdatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new Wanderstein
                {
                    Name = "Seeufer",
                    UniqueId = "WS-2024-006",
                    PreviewUrl = "https://picsum.photos/300/200?random=6",
                    Description = "Hiking stone directly at the picturesque lakeshore",
                    Location = "Lake Chiemsee, Bavaria",
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    UpdatedAt = DateTime.UtcNow.AddDays(-1)
                }
            };

            _context.Wandersteine.AddRange(sampleStones);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Successfully seeded {sampleStones.Count} Wandersteine");
        }
    }
}