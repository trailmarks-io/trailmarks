using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace TrailmarksApi.Data
{
    /// <summary>
    /// Helper class for configuring DbContext options consistently across the application
    /// </summary>
    public static class DbContextOptionsHelper
    {
        /// <summary>
        /// Configures Npgsql with standard options including NetTopologySuite for PostGIS support
        /// </summary>
        /// <param name="npgsqlOptions">Npgsql options builder</param>
        public static void ConfigureNpgsql(NpgsqlDbContextOptionsBuilder npgsqlOptions)
        {
            // Enable NetTopologySuite for PostGIS spatial data types
            npgsqlOptions.UseNetTopologySuite();
        }
    }
}
