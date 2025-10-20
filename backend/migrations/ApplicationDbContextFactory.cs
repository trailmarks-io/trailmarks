using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TrailmarksApi.Data;

namespace TrailmarksApi.Migrations
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
            var connectionString = Environment.GetEnvironmentVariable("MIGRATIONS_CONNECTION_STRING") 
                ?? "Host=localhost;Database=trailmarks_migrations;Username=postgres;Password=postgres";
            
            optionsBuilder.UseNpgsql(connectionString, x => x.MigrationsAssembly("TrailmarksApi.Migrations"));
            
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
