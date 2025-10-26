using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace TrailmarksApi.Models
{
    /// <summary>
    /// Represents a hiking stone with its metadata
    /// </summary>
    public class Wanderstein
    {
        private const int WGS84_SRID = 4326;
        private static readonly GeometryFactory GeometryFactory = 
            NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(srid: WGS84_SRID);

        /// <summary>
        /// Unique identifier for the Wanderstein
        /// </summary>
        [Key]
        public uint Id { get; set; }

        /// <summary>
        /// Name of the Wanderstein
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Unique identifier string (e.g., WS-2024-001)
        /// </summary>
        [Required]
        [MaxLength(50)]
        [Column("unique_id")]
        public string UniqueId { get; set; } = string.Empty;

        /// <summary>
        /// URL to the preview image
        /// </summary>
        [MaxLength(500)]
        [Column("preview_url")]
        public string PreviewUrl { get; set; } = string.Empty;

        /// <summary>
        /// Description of the Wanderstein
        /// </summary>
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Location description
        /// </summary>
        [MaxLength(200)]
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// Geographic coordinates (WGS84) - stored as PostGIS Point in database
        /// </summary>
        [NotMapped]
        public GeoCoordinate? Coordinates 
        { 
            get => LocationPoint == null || LocationPoint.IsEmpty
                ? null 
                : new GeoCoordinate(LocationPoint.Y, LocationPoint.X);
            set => LocationPoint = value == null 
                ? null 
                : GeometryFactory.CreatePoint(new Coordinate(value.Longitude, value.Latitude));
        }

        /// <summary>
        /// Internal PostGIS Point column for spatial queries
        /// </summary>
        [Column("Location_Point")]
        public Point? LocationPoint { get; set; }

        /// <summary>
        /// Creation timestamp
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Update timestamp
        /// </summary>
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Response DTO for API endpoints
    /// </summary>
    public class WandersteinResponse
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        public uint Id { get; set; }

        /// <summary>
        /// Name of the Wanderstein
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Unique identifier string
        /// </summary>
        public string Unique_Id { get; set; } = string.Empty;

        /// <summary>
        /// URL to the preview image
        /// </summary>
        public string Preview_Url { get; set; } = string.Empty;

        /// <summary>
        /// Creation timestamp in ISO 8601 format
        /// </summary>
        public string Created_At { get; set; } = string.Empty;

        /// <summary>
        /// Latitude coordinate (WGS84)
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// Longitude coordinate (WGS84)
        /// </summary>
        public double? Longitude { get; set; }

        /// <summary>
        /// Location description
        /// </summary>
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// Converts a <see cref="Wanderstein"/> entity into a <see cref="WandersteinResponse"/> DTO.
        /// </summary>
        /// <param name="wanderstein">The source entity to convert.</param>
        /// <returns>A <see cref="WandersteinResponse"/> populated from the source entity. The Created_At value is formatted as "yyyy-MM-ddTHH:mm:ssZ". Latitude and Longitude are taken from the entity's Coordinates when present; otherwise they are null.</returns>
        public static WandersteinResponse FromEntity(Wanderstein wanderstein)
        {
            return new WandersteinResponse
            {
                Id = wanderstein.Id,
                Name = wanderstein.Name,
                Unique_Id = wanderstein.UniqueId,
                Preview_Url = wanderstein.PreviewUrl,
                Created_At = wanderstein.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                Latitude = wanderstein.Coordinates?.Latitude,
                Longitude = wanderstein.Coordinates?.Longitude,
                Location = wanderstein.Location
            };
        }
    }

    /// <summary>
    /// Detailed response DTO for individual Wanderstein endpoints
    /// </summary>
    public class WandersteinDetailResponse
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        public uint Id { get; set; }

        /// <summary>
        /// Name of the Wanderstein
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Unique identifier string
        /// </summary>
        public string Unique_Id { get; set; } = string.Empty;

        /// <summary>
        /// URL to the preview image
        /// </summary>
        public string Preview_Url { get; set; } = string.Empty;

        /// <summary>
        /// Description of the Wanderstein
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Location description
        /// </summary>
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// Creation timestamp in ISO 8601 format
        /// </summary>
        public string Created_At { get; set; } = string.Empty;

        /// <summary>
        /// Update timestamp in ISO 8601 format
        /// </summary>
        public string Updated_At { get; set; } = string.Empty;

        /// <summary>
        /// Latitude coordinate (WGS84)
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// Longitude coordinate (WGS84)
        /// </summary>
        public double? Longitude { get; set; }

        /// <summary>
        /// Converts a <see cref="Wanderstein"/> entity into a <see cref="WandersteinDetailResponse"/> DTO.
        /// </summary>
        /// <param name="wanderstein">The source entity to convert.</param>
        /// <returns>A <see cref="WandersteinDetailResponse"/> populated from the source entity.</returns>
        public static WandersteinDetailResponse FromEntity(Wanderstein wanderstein)
        {
            return new WandersteinDetailResponse
            {
                Id = wanderstein.Id,
                Name = wanderstein.Name,
                Unique_Id = wanderstein.UniqueId,
                Preview_Url = wanderstein.PreviewUrl,
                Description = wanderstein.Description,
                Location = wanderstein.Location,
                Created_At = wanderstein.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                Updated_At = wanderstein.UpdatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                Latitude = wanderstein.Coordinates?.Latitude,
                Longitude = wanderstein.Coordinates?.Longitude
            };
        }
    }
}