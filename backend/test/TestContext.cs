using Microsoft.EntityFrameworkCore;
using TrailmarksApi.Data;

namespace TrailmarksApi.Tests
{
    /// <summary>
    /// Base class for tests that require database context
    /// </summary>
    public abstract class TestContext
    {
        /// <summary>
        /// Creates an in-memory database context for testing
        /// </summary>
        /// <returns>A new ApplicationDbContext configured with in-memory database</returns>
        protected ApplicationDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}
