using Microsoft.AspNetCore.Mvc;
using TrailmarksApi.Controllers;

namespace TrailmarksApi.Tests.Controllers
{
    public class HealthControllerTests
    {
        [Fact]
        public void GetHealth_ReturnsOkResult()
        {
            // Arrange
            var controller = new HealthController();

            // Act
            var result = controller.GetHealth();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public void GetHealth_ReturnsHealthyStatus()
        {
            // Arrange
            var controller = new HealthController();

            // Act
            var result = controller.GetHealth() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var value = result.Value;
            Assert.NotNull(value);
            
            var statusProperty = value.GetType().GetProperty("status");
            Assert.NotNull(statusProperty);
            Assert.Equal("healthy", statusProperty.GetValue(value));
        }

        [Fact]
        public void GetHealth_ReturnsCorrectServiceName()
        {
            // Arrange
            var controller = new HealthController();

            // Act
            var result = controller.GetHealth() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var value = result.Value;
            Assert.NotNull(value);
            
            var serviceProperty = value.GetType().GetProperty("service");
            Assert.NotNull(serviceProperty);
            Assert.Equal("trailmarks-backend", serviceProperty.GetValue(value));
        }
    }
}
