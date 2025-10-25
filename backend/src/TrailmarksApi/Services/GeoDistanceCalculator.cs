using TrailmarksApi.Models;

namespace TrailmarksApi.Services
{
    /// <summary>
    /// Provides geographic distance calculations using the Haversine formula
    /// </summary>
    public static class GeoDistanceCalculator
    {
        private const double EarthRadiusKm = 6371.0;

        /// <summary>
        /// Calculates the great-circle distance between two points on Earth using the Haversine formula
        /// </summary>
        /// <param name="coord1">First coordinate</param>
        /// <param name="coord2">Second coordinate</param>
        /// <returns>Distance in kilometers</returns>
        public static double CalculateDistanceKm(GeoCoordinate coord1, GeoCoordinate coord2)
        {
            var lat1Rad = DegreesToRadians(coord1.Latitude);
            var lat2Rad = DegreesToRadians(coord2.Latitude);
            var deltaLat = DegreesToRadians(coord2.Latitude - coord1.Latitude);
            var deltaLon = DegreesToRadians(coord2.Longitude - coord1.Longitude);

            var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                    Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                    Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);
            
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            
            return EarthRadiusKm * c;
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
    }
}
