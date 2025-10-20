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
                // Ensure the database exists, then apply migrations
                // MigrateAsync will create the database if it doesn't exist and apply all pending migrations
                _logger.LogInformation("Checking database and applying migrations...");
                await _context.Database.MigrateAsync();
                _logger.LogInformation("Database migrations applied successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initializing the database");
                throw;
            }
        }
    }
}