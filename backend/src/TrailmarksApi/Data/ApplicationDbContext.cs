using Microsoft.EntityFrameworkCore;
using TrailmarksApi.Models;

namespace TrailmarksApi.Data
{
    /// <summary>
    /// Database context for the Trailmarks application
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Wandersteine dataset
        /// </summary>
        public DbSet<Wanderstein> Wandersteine { get; set; }

        /// <summary>
        /// Translations dataset
        /// </summary>
        public DbSet<Translation> Translations { get; set; }

        /// <summary>
        /// Configures the Entity Framework Core model for application entities.
        /// </summary>
        /// <param name="modelBuilder">The builder used to configure entity types including Wanderstein (properties, keys, indexes, and owned Coordinates mapped to Latitude/Longitude columns) and Translation (properties, keys, and composite unique index).</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Enable PostGIS extension
            modelBuilder.HasPostgresExtension("postgis");

            // Configure Wanderstein entity
            modelBuilder.Entity<Wanderstein>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UniqueId).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.UniqueId).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PreviewUrl).HasMaxLength(500);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Location).HasMaxLength(200);
                
                // Configure GeoCoordinate as owned type
                entity.OwnsOne(e => e.Coordinates, coordinates =>
                {
                    coordinates.Property(c => c.Latitude).HasColumnName("Latitude");
                    coordinates.Property(c => c.Longitude).HasColumnName("Longitude");
                });
            });

            // Configure Translation entity
            modelBuilder.Entity<Translation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.Key, e.Language }).IsUnique();
                entity.Property(e => e.Key).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Language).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Value).IsRequired().HasMaxLength(500);
            });
        }
    }
}