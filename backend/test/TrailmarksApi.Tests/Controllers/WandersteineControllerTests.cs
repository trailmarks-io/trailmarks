using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TrailmarksApi.Controllers;
using TrailmarksApi.Data;
using TrailmarksApi.Models;

namespace TrailmarksApi.Tests.Controllers
{
    public class WandersteineControllerTests : IAsyncLifetime
    {
        private ApplicationDbContext? _context;

        public async Task InitializeAsync()
        {
            _context = await DatabaseFixture.CreatePostgreSqlContextAsync();
        }

        public async Task DisposeAsync()
        {
            if (_context != null)
            {
                await _context.Database.EnsureDeletedAsync();
                await _context.DisposeAsync();
            }
        }

        [Fact]
        public async Task GetRecentWandersteine_ReturnsOkResult()
        {
            // Arrange
            var logger = new Mock<ILogger<WandersteineController>>();
            var controller = new WandersteineController(_context!, logger.Object);

            // Act
            var result = await controller.GetRecentWandersteine();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetRecentWandersteine_ReturnsMaximumFiveItems()
        {
            // Arrange
            // Add 7 test items
            for (int i = 1; i <= 7; i++)
            {
                _context!.Wandersteine.Add(new Wanderstein
                {
                    Name = $"Test Stone {i}",
                    UniqueId = $"WS-TEST-{i:D3}",
                    PreviewUrl = $"https://example.com/image{i}.jpg",
                    CreatedAt = DateTime.UtcNow.AddDays(-i)
                });
            }
            await _context!.SaveChangesAsync();

            var logger = new Mock<ILogger<WandersteineController>>();
            var controller = new WandersteineController(_context, logger.Object);

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
            // Clear existing seed data for this test
            _context!.Wandersteine.RemoveRange(_context.Wandersteine);
            await _context.SaveChangesAsync();

            _context.Wandersteine.Add(new Wanderstein
            {
                Name = "Oldest",
                UniqueId = "WS-001",
                PreviewUrl = "https://example.com/1.jpg",
                CreatedAt = DateTime.UtcNow.AddDays(-10)
            });
            _context.Wandersteine.Add(new Wanderstein
            {
                Name = "Newest",
                UniqueId = "WS-002",
                PreviewUrl = "https://example.com/2.jpg",
                CreatedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();

            var logger = new Mock<ILogger<WandersteineController>>();
            var controller = new WandersteineController(_context, logger.Object);

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
            var logger = new Mock<ILogger<WandersteineController>>();
            var controller = new WandersteineController(_context!, logger.Object);

            // Act
            var result = await controller.GetAllWandersteine();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAllWandersteine_ReturnsAllItems()
        {
            // Arrange
            // Clear existing seed data for this test
            _context!.Wandersteine.RemoveRange(_context.Wandersteine);
            await _context.SaveChangesAsync();

            for (int i = 1; i <= 10; i++)
            {
                _context.Wandersteine.Add(new Wanderstein
                {
                    Name = $"Stone {i}",
                    UniqueId = $"WS-{i:D3}",
                    PreviewUrl = $"https://example.com/{i}.jpg",
                    CreatedAt = DateTime.UtcNow.AddDays(-i)
                });
            }
            await _context.SaveChangesAsync();

            var logger = new Mock<ILogger<WandersteineController>>();
            var controller = new WandersteineController(_context, logger.Object);

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
            // Clear existing seed data for this test
            _context!.Wandersteine.RemoveRange(_context.Wandersteine);
            await _context.SaveChangesAsync();

            var logger = new Mock<ILogger<WandersteineController>>();
            var controller = new WandersteineController(_context, logger.Object);

            // Act
            var result = await controller.GetAllWandersteine() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var wandersteine = Assert.IsAssignableFrom<IEnumerable<WandersteinResponse>>(result.Value);
            Assert.Empty(wandersteine);
        }

        [Fact]
        public async Task GetWandersteinByUniqueId_ReturnsOkResultWithCorrectItem()
        {
            // Arrange
            _context!.Wandersteine.RemoveRange(_context.Wandersteine);
            await _context.SaveChangesAsync();

            var testWanderstein = new Wanderstein
            {
                Name = "Test Stone",
                UniqueId = "WS-TEST-001",
                PreviewUrl = "https://example.com/test.jpg",
                Description = "Test Description",
                Location = "Test Location",
                Coordinates = new GeoCoordinate(48.137154, 11.576124),
                CreatedAt = DateTime.UtcNow
            };
            _context.Wandersteine.Add(testWanderstein);
            await _context.SaveChangesAsync();

            var logger = new Mock<ILogger<WandersteineController>>();
            var controller = new WandersteineController(_context, logger.Object);

            // Act
            var result = await controller.GetWandersteinByUniqueId("WS-TEST-001");

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            var wanderstein = Assert.IsType<WandersteinDetailResponse>(okResult!.Value);
            Assert.Equal("Test Stone", wanderstein.Name);
            Assert.Equal("WS-TEST-001", wanderstein.Unique_Id);
            Assert.Equal("Test Description", wanderstein.Description);
            Assert.Equal("Test Location", wanderstein.Location);
            Assert.Equal(48.137154, wanderstein.Latitude);
            Assert.Equal(11.576124, wanderstein.Longitude);
        }

        [Fact]
        public async Task GetWandersteinByUniqueId_ReturnsNotFoundForNonExistentId()
        {
            // Arrange
            var logger = new Mock<ILogger<WandersteineController>>();
            var controller = new WandersteineController(_context!, logger.Object);

            // Act
            var result = await controller.GetWandersteinByUniqueId("WS-DOES-NOT-EXIST");

            // Assert
            Assert.IsType<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Equal(404, objectResult!.StatusCode);
        }

        [Fact]
        public async Task GetWandersteinByUniqueId_ReturnsLocationDescription()
        {
            // Arrange
            _context!.Wandersteine.RemoveRange(_context.Wandersteine);
            await _context.SaveChangesAsync();

            var testWanderstein = new Wanderstein
            {
                Name = "Stone with Location",
                UniqueId = "WS-LOC-001",
                PreviewUrl = "https://example.com/loc.jpg",
                Location = "Near the old oak tree, 500m from the parking lot",
                CreatedAt = DateTime.UtcNow
            };
            _context.Wandersteine.Add(testWanderstein);
            await _context.SaveChangesAsync();

            var logger = new Mock<ILogger<WandersteineController>>();
            var controller = new WandersteineController(_context, logger.Object);

            // Act
            var result = await controller.GetWandersteinByUniqueId("WS-LOC-001");

            // Assert
            var okResult = result as OkObjectResult;
            var wanderstein = Assert.IsType<WandersteinDetailResponse>(okResult!.Value);
            Assert.Equal("Near the old oak tree, 500m from the parking lot", wanderstein.Location);
        }
    }
}
