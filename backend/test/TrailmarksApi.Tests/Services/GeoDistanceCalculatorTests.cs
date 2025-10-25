using TrailmarksApi.Models;
using TrailmarksApi.Services;

namespace TrailmarksApi.Tests.Services
{
    public class GeoDistanceCalculatorTests
    {
        [Fact]
        public void CalculateDistanceKm_SamePoint_ReturnsZero()
        {
            // Arrange
            var coord1 = new GeoCoordinate(51.4818, 7.2162); // Bochum
            var coord2 = new GeoCoordinate(51.4818, 7.2162); // Bochum

            // Act
            var distance = GeoDistanceCalculator.CalculateDistanceKm(coord1, coord2);

            // Assert
            Assert.Equal(0, distance, 2); // Within 0.01 km
        }

        [Fact]
        public void CalculateDistanceKm_BochumToEssen_ReturnsCorrectDistance()
        {
            // Arrange
            var bochum = new GeoCoordinate(51.4818, 7.2162);
            var essen = new GeoCoordinate(51.4556, 7.0116);

            // Act
            var distance = GeoDistanceCalculator.CalculateDistanceKm(bochum, essen);

            // Assert
            // Actual distance is approximately 14.5 km
            Assert.True(distance >= 14.0 && distance <= 15.0);
        }

        [Fact]
        public void CalculateDistanceKm_BerlinToMunich_ReturnsCorrectDistance()
        {
            // Arrange
            var berlin = new GeoCoordinate(52.5200, 13.4050);
            var munich = new GeoCoordinate(48.1351, 11.5820);

            // Act
            var distance = GeoDistanceCalculator.CalculateDistanceKm(berlin, munich);

            // Assert
            // Actual distance is approximately 504 km
            Assert.True(distance >= 500 && distance <= 510);
        }

        [Fact]
        public void CalculateDistanceKm_SymmetricDistance()
        {
            // Arrange
            var coord1 = new GeoCoordinate(51.4818, 7.2162);
            var coord2 = new GeoCoordinate(48.1351, 11.5820);

            // Act
            var distance1 = GeoDistanceCalculator.CalculateDistanceKm(coord1, coord2);
            var distance2 = GeoDistanceCalculator.CalculateDistanceKm(coord2, coord1);

            // Assert
            Assert.Equal(distance1, distance2, 5);
        }

        [Fact]
        public void CalculateDistanceKm_AcrossEquator_ReturnsCorrectDistance()
        {
            // Arrange
            var northernPoint = new GeoCoordinate(10.0, 0.0);
            var southernPoint = new GeoCoordinate(-10.0, 0.0);

            // Act
            var distance = GeoDistanceCalculator.CalculateDistanceKm(northernPoint, southernPoint);

            // Assert
            // 20 degrees latitude ≈ 2222 km
            Assert.True(distance >= 2200 && distance <= 2250);
        }
    }
}
