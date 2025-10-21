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
            
            optionsBuilder.UseNpgsql(connectionString);
            
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
