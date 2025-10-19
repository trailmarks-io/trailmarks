namespace TrailmarksApi.Models
{
    /// <summary>
    /// Value type representing geographic coordinates (WGS84)
    /// </summary>
    public record GeoCoordinate
    {
        /// <summary>
        /// Latitude coordinate
        /// </summary>
        public double Latitude { get; init; }

        /// <summary>
        /// Longitude coordinate
        /// </summary>
        public double Longitude { get; init; }

        /// <summary>
        /// Creates a new GeoCoordinate instance
        /// </summary>
        /// <param name="latitude">Latitude in decimal degrees</param>
        /// <param name="longitude">Longitude in decimal degrees</param>
        public GeoCoordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// Default constructor for EF Core
        /// </summary>
        public GeoCoordinate() : this(0, 0)
        {
        }

        /// <summary>
        /// Validates if the coordinates are within valid WGS84 ranges
        /// </summary>
        public bool IsValid()
        {
            return Latitude >= -90 && Latitude <= 90 && Longitude >= -180 && Longitude <= 180;
        }
    }
}
