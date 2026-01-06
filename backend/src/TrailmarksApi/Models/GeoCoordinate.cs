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
        /// Initializes a GeoCoordinate with the specified latitude and longitude in decimal degrees (WGS84).
        /// </summary>
        /// <param name="latitude">Latitude in decimal degrees; expected range is -90 to 90.</param>
        /// <param name="longitude">Longitude in decimal degrees; expected range is -180 to 180.</param>
        public GeoCoordinate(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// Initializes a new GeoCoordinate with latitude and longitude set to 0.
        /// </summary>
        /// <remarks>
        /// Provided for ORM/EF Core compatibility.
        /// </remarks>
        public GeoCoordinate() : this(0, 0)
        {
        }

        /// <summary>
        /// Determines whether the coordinate lies within valid WGS84 bounds.
        /// </summary>
        /// <returns><c>true</c> if Latitude is between -90 and 90 inclusive and Longitude is between -180 and 180 inclusive; <c>false</c> otherwise.</returns>
        public bool IsValid()
        {
            return Latitude >= -90 && Latitude <= 90 && Longitude >= -180 && Longitude <= 180;
        }
    }
}