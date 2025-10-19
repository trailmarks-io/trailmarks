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
        /// Initialize the database by applying all pending migrations
        /// </summary>
        public async Task InitializeAsync()
        {
            try
            {
                // Apply all pending migrations
                await _context.Database.MigrateAsync();
                
                _logger.LogInformation("Database initialized successfully with all migrations applied");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initializing the database");
                throw;
            }
        }

    }
}