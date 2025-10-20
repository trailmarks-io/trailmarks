using Microsoft.EntityFrameworkCore;
using TrailmarksApi.Data;

namespace TrailmarksApi.Services
{
    /// <summary>
    /// Service for database initialization using EF Core migrations
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
        /// Initialize the database by applying pending migrations
        /// </summary>
        public async Task InitializeAsync()
        {
            try
            {
                // Apply all pending migrations
                var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
                if (pendingMigrations.Any())
                {
                    _logger.LogInformation("Applying {Count} pending migrations", pendingMigrations.Count());
                    await _context.Database.MigrateAsync();
                    _logger.LogInformation("Database migrations applied successfully");
                }
                else
                {
                    _logger.LogInformation("Database is up to date, no pending migrations");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initializing the database");
                throw;
            }
        }
    }
}