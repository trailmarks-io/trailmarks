using Microsoft.EntityFrameworkCore;
using TrailmarksApi.Data;

namespace TrailmarksApi.Tests
{
    /// <summary>
    /// Helper class for creating test database contexts (Composition over Inheritance)
    /// </summary>
    public static class DatabaseFixture
    {
        /// <summary>
        /// Creates an in-memory SQLite database context for testing with migrations and seed data
        /// </summary>
        /// <returns>A new ApplicationDbContext configured with in-memory SQLite database with seed data</returns>
        public static ApplicationDbContext CreateInMemoryContext()
        {
            return CreateInMemoryContext(applySeedMigrations: true);
        }

        /// <summary>
        /// Creates an in-memory SQLite database context for testing with migrations support
        /// </summary>
        /// <param name="applySeedMigrations">If true, applies all migrations including seed data. If false, only creates schema.</param>
        /// <returns>A new ApplicationDbContext configured with in-memory SQLite database</returns>
        public static ApplicationDbContext CreateInMemoryContext(bool applySeedMigrations)
        {
            var connectionString = $"Data Source=test_{Guid.NewGuid()};Mode=Memory;Cache=Shared;";
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connectionString,
                    b => b.MigrationsAssembly("TrailmarksApi.Migrations"))
                .Options;

            var context = new ApplicationDbContext(options);
            
            // Open the connection to keep the in-memory database alive
            context.Database.OpenConnection();
            
            if (applySeedMigrations)
            {
                // Apply all migrations including seed data
                context.Database.Migrate();
            }
            else
            {
                // Only create the schema without seed data
                context.Database.EnsureCreated();
            }

            return context;
        }
    }
}
