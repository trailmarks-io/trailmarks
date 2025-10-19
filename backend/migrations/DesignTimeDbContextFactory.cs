using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TrailmarksApi.Data;

namespace TrailmarksApi.Migrations
{
    /// <summary>
    /// Factory for creating ApplicationDbContext at design time for migrations
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            
            // Use SQLite for design-time migrations by default
            // The actual database provider will be configured at runtime
            optionsBuilder.UseSqlite("Data Source=trailmarks.db",
                b => b.MigrationsAssembly("TrailmarksApi.Migrations"));
            
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
