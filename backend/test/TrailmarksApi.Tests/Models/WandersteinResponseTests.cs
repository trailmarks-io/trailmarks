using TrailmarksApi.Models;

namespace TrailmarksApi.Tests.Models
{
    public class WandersteinResponseTests
    {
        [Fact]
        public void FromEntity_MapsAllProperties()
        {
            // Arrange
            var wanderstein = new Wanderstein
            {
                Id = 1,
                Name = "Test Stone",
                UniqueId = "WS-TEST-001",
                PreviewUrl = "https://example.com/image.jpg",
                Description = "A test stone",
                Location = "Test Location",
                CreatedAt = new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2024, 1, 16, 10, 30, 0, DateTimeKind.Utc)
            };

            // Act
            var response = WandersteinResponse.FromEntity(wanderstein);

            // Assert
            Assert.Equal(wanderstein.Id, response.Id);
            Assert.Equal(wanderstein.Name, response.Name);
            Assert.Equal(wanderstein.UniqueId, response.Unique_Id);
            Assert.Equal(wanderstein.PreviewUrl, response.Preview_Url);
        }

        [Fact]
        public void FromEntity_FormatsCreatedAtInISO8601()
        {
            // Arrange
            var wanderstein = new Wanderstein
            {
                Id = 1,
                Name = "Test",
                UniqueId = "WS-001",
                PreviewUrl = "https://example.com/image.jpg",
                CreatedAt = new DateTime(2024, 8, 4, 12, 0, 0, DateTimeKind.Utc)
            };

            // Act
            var response = WandersteinResponse.FromEntity(wanderstein);

            // Assert
            Assert.Equal("2024-08-04T12:00:00Z", response.Created_At);
        }

        [Fact]
        public void FromEntity_HandlesEmptyStrings()
        {
            // Arrange
            var wanderstein = new Wanderstein
            {
                Id = 1,
                Name = "",
                UniqueId = "",
                PreviewUrl = "",
                CreatedAt = DateTime.UtcNow
            };

            // Act
            var response = WandersteinResponse.FromEntity(wanderstein);

            // Assert
            Assert.Equal("", response.Name);
            Assert.Equal("", response.Unique_Id);
            Assert.Equal("", response.Preview_Url);
        }

        [Fact]
        public void FromEntity_PreservesIdValue()
        {
            // Arrange
            var wanderstein = new Wanderstein
            {
                Id = 999,
                Name = "Test",
                UniqueId = "WS-999",
                PreviewUrl = "https://example.com/999.jpg",
                CreatedAt = DateTime.UtcNow
            };

            // Act
            var response = WandersteinResponse.FromEntity(wanderstein);

            // Assert
            Assert.Equal(999u, response.Id);
        }
    }
}
