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
        /// Creates an in-memory database context for testing
        /// </summary>
        /// <returns>A new ApplicationDbContext configured with in-memory database</returns>
        public static ApplicationDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}
