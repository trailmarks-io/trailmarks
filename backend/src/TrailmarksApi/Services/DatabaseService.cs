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

        /// <summary>
        /// Creates a new DatabaseService using the provided database context and logger.
        /// </summary>
        /// <param name="context">The application's EF Core <see cref="ApplicationDbContext"/> used to check and apply migrations.</param>
        /// <param name="logger">An <see cref="ILogger{DatabaseService}"/> used to record informational and error events during initialization.</param>
        public DatabaseService(ApplicationDbContext context, ILogger<DatabaseService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Initializes the application's database by applying any pending Entity Framework Core migrations.
        /// </summary>
        /// <exception cref="Exception">Propagates any exception that occurs while checking or applying migrations.</exception>
        public async Task InitializeAsync()
        {
            try
            {
                _logger.LogInformation("Starting database initialization...");
                
                // Check if database can be connected
                var canConnect = await _context.Database.CanConnectAsync();
                _logger.LogInformation("Database connection check: {CanConnect}", canConnect);
                
                // Get pending migrations
                var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
                var pendingList = pendingMigrations.ToList();
                _logger.LogInformation("Found {Count} pending migrations: {Migrations}", 
                    pendingList.Count, 
                    string.Join(", ", pendingList));
                
                // Get applied migrations
                var appliedMigrations = await _context.Database.GetAppliedMigrationsAsync();
                var appliedList = appliedMigrations.ToList();
                _logger.LogInformation("Found {Count} already applied migrations: {Migrations}", 
                    appliedList.Count, 
                    string.Join(", ", appliedList));
                
                // Apply migrations
                _logger.LogInformation("Applying database migrations...");
                await _context.Database.MigrateAsync();
                _logger.LogInformation("Database migrations completed successfully");
                
                // Verify migrations were applied
                var remainingPending = await _context.Database.GetPendingMigrationsAsync();
                var remainingList = remainingPending.ToList();
                if (remainingList.Any())
                {
                    _logger.LogWarning("After migration, {Count} migrations are still pending: {Migrations}", 
                        remainingList.Count, 
                        string.Join(", ", remainingList));
                }
                else
                {
                    _logger.LogInformation("All migrations have been applied successfully");
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