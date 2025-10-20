using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using TrailmarksApi.Data;

namespace TrailmarksApi.Tests
{
    /// <summary>
    /// Helper class for creating test database contexts (Composition over Inheritance)
    /// </summary>
    public static class DatabaseFixture
    {
        private static readonly PostgreSqlContainer _postgresContainer = new PostgreSqlBuilder()
            .WithImage("postgres:16-alpine")
            .WithDatabase("trailmarks_test")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

        private static bool _containerStarted = false;
        private static readonly object _lock = new object();

        /// <summary>
        /// Ensures the PostgreSQL container is started (called once per test run)
        /// </summary>
        public static async Task EnsureContainerStartedAsync()
        {
            if (!_containerStarted)
            {
                lock (_lock)
                {
                    if (!_containerStarted)
                    {
                        _postgresContainer.StartAsync().GetAwaiter().GetResult();
                        _containerStarted = true;
                    }
                }
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// Creates a PostgreSQL database context for testing with a unique database name
        /// </summary>
        /// <returns>A new ApplicationDbContext configured with PostgreSQL test database</returns>
        public static async Task<ApplicationDbContext> CreatePostgreSqlContextAsync()
        {
            await EnsureContainerStartedAsync();

            // Create a unique database name for each test to avoid conflicts
            var uniqueDbName = $"test_db_{Guid.NewGuid():N}";
            
            // Get connection string and replace database name
            var connectionString = _postgresContainer.GetConnectionString()
                .Replace("trailmarks_test", uniqueDbName);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(connectionString, x => x.MigrationsAssembly("TrailmarksApi.Migrations"))
                .Options;

            var context = new ApplicationDbContext(options);
            
            // Apply pending migrations
            await context.Database.MigrateAsync();
            
            return context;
        }

        /// <summary>
        /// Stops the PostgreSQL container (typically called during test cleanup)
        /// </summary>
        public static async Task StopContainerAsync()
        {
            if (_containerStarted)
            {
                await _postgresContainer.StopAsync();
                _containerStarted = false;
            }
        }
    }
}
