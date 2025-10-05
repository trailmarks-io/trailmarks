using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TrailmarksApi.Controllers;
using TrailmarksApi.Models;

namespace TrailmarksApi.Tests.Controllers
{
    public class WandersteineControllerTests : TestContext
    {

        [Fact]
        public async Task GetRecentWandersteine_ReturnsOkResult()
        {
            // Arrange
            var context = GetInMemoryContext();
            var logger = new Mock<ILogger<WandersteineController>>();
            var controller = new WandersteineController(context, logger.Object);

            // Act
            var result = await controller.GetRecentWandersteine();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetRecentWandersteine_ReturnsMaximumFiveItems()
        {
            // Arrange
            var context = GetInMemoryContext();
            
            // Add 7 test items
            for (int i = 1; i <= 7; i++)
            {
                context.Wandersteine.Add(new Wanderstein
                {
                    Name = $"Test Stone {i}",
                    UniqueId = $"WS-TEST-{i:D3}",
                    PreviewUrl = $"https://example.com/image{i}.jpg",
                    CreatedAt = DateTime.UtcNow.AddDays(-i)
                });
            }
            await context.SaveChangesAsync();

            var logger = new Mock<ILogger<WandersteineController>>();
            var controller = new WandersteineController(context, logger.Object);

            // Act
            var result = await controller.GetRecentWandersteine() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var wandersteine = Assert.IsAssignableFrom<IEnumerable<WandersteinResponse>>(result.Value);
            Assert.Equal(5, wandersteine.Count());
        }

        [Fact]
        public async Task GetRecentWandersteine_ReturnsItemsOrderedByCreatedAtDescending()
        {
            // Arrange
            var context = GetInMemoryContext();
            
            context.Wandersteine.Add(new Wanderstein
            {
                Name = "Oldest",
                UniqueId = "WS-001",
                PreviewUrl = "https://example.com/1.jpg",
                CreatedAt = DateTime.UtcNow.AddDays(-10)
            });
            context.Wandersteine.Add(new Wanderstein
            {
                Name = "Newest",
                UniqueId = "WS-002",
                PreviewUrl = "https://example.com/2.jpg",
                CreatedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync();

            var logger = new Mock<ILogger<WandersteineController>>();
            var controller = new WandersteineController(context, logger.Object);

            // Act
            var result = await controller.GetRecentWandersteine() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var wandersteine = Assert.IsAssignableFrom<IEnumerable<WandersteinResponse>>(result.Value).ToList();
            Assert.Equal("Newest", wandersteine.First().Name);
            Assert.Equal("Oldest", wandersteine.Last().Name);
        }

        [Fact]
        public async Task GetAllWandersteine_ReturnsOkResult()
        {
            // Arrange
            var context = GetInMemoryContext();
            var logger = new Mock<ILogger<WandersteineController>>();
            var controller = new WandersteineController(context, logger.Object);

            // Act
            var result = await controller.GetAllWandersteine();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAllWandersteine_ReturnsAllItems()
        {
            // Arrange
            var context = GetInMemoryContext();
            
            for (int i = 1; i <= 10; i++)
            {
                context.Wandersteine.Add(new Wanderstein
                {
                    Name = $"Stone {i}",
                    UniqueId = $"WS-{i:D3}",
                    PreviewUrl = $"https://example.com/{i}.jpg",
                    CreatedAt = DateTime.UtcNow.AddDays(-i)
                });
            }
            await context.SaveChangesAsync();

            var logger = new Mock<ILogger<WandersteineController>>();
            var controller = new WandersteineController(context, logger.Object);

            // Act
            var result = await controller.GetAllWandersteine() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var wandersteine = Assert.IsAssignableFrom<IEnumerable<WandersteinResponse>>(result.Value);
            Assert.Equal(10, wandersteine.Count());
        }

        [Fact]
        public async Task GetAllWandersteine_ReturnsEmptyListWhenNoData()
        {
            // Arrange
            var context = GetInMemoryContext();
            var logger = new Mock<ILogger<WandersteineController>>();
            var controller = new WandersteineController(context, logger.Object);

            // Act
            var result = await controller.GetAllWandersteine() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var wandersteine = Assert.IsAssignableFrom<IEnumerable<WandersteinResponse>>(result.Value);
            Assert.Empty(wandersteine);
        }
    }
}
