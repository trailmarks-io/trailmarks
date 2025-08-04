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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
            });
        }
    }
}