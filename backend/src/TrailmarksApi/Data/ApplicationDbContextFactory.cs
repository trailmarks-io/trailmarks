using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TrailmarksApi.Data;

namespace TrailmarksApi.Data
{
    /// <summary>
    /// Design-time factory for ApplicationDbContext to enable EF Core migrations
    /// </summary>
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        /// <summary>
        /// Creates an <see cref="ApplicationDbContext"/> configured for design-time operations used by EF Core tools.
        /// </summary>
        /// <param name="args">Command-line arguments provided by the design-time host (ignored by this implementation).</param>
        /// <returns>An <see cref="ApplicationDbContext"/> configured to use PostgreSQL with the connection string taken from the MIGRATIONS_CONNECTION_STRING environment variable.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the MIGRATIONS_CONNECTION_STRING environment variable is not set.</exception>
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            
            // Use a connection string for design-time operations
            // This will be overridden at runtime with the actual connection string
            // For local development, set the MIGRATIONS_CONNECTION_STRING environment variable
            var connectionString = Environment.GetEnvironmentVariable("MIGRATIONS_CONNECTION_STRING") 
                ?? throw new InvalidOperationException(
                    "MIGRATIONS_CONNECTION_STRING environment variable is not set. " +
                    "Please set it to a valid PostgreSQL connection string for migrations. " +
                    "For guidance on secure connection string formats, see: https://www.npgsql.org/doc/connection-string-parameters.html. " +
                    "Avoid including plaintext passwords; use environment variables or a secure secret manager.");
            
            optionsBuilder.UseNpgsql(connectionString, DbContextOptionsHelper.ConfigureNpgsql);
            
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}