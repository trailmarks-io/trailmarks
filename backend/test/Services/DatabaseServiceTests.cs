using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TrailmarksApi.Data;
using TrailmarksApi.Services;

namespace TrailmarksApi.Tests.Services
{
    public class DatabaseServiceTests : IAsyncLifetime
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
        public async Task InitializeAsync_ExecutesSuccessfully()
        {
            // Arrange
            var logger = new Mock<ILogger<DatabaseService>>();
            var service = new DatabaseService(_context!, logger.Object);

            // Act
            await service.InitializeAsync();

            // Assert
            // Verify context has data seeded
            Assert.True(await _context!.Wandersteine.AnyAsync());
        }

        [Fact]
        public async Task InitializeAsync_SeedsWandersteine()
        {
            // Arrange
            var logger = new Mock<ILogger<DatabaseService>>();
            var service = new DatabaseService(_context!, logger.Object);

            // Act
            await service.InitializeAsync();

            // Assert
            var wandersteine = await _context!.Wandersteine.ToListAsync();
            Assert.NotEmpty(wandersteine);
            Assert.All(wandersteine, w =>
            {
                Assert.False(string.IsNullOrEmpty(w.Name));
                Assert.False(string.IsNullOrEmpty(w.UniqueId));
            });
        }

        [Fact]
        public async Task InitializeAsync_SeedsTranslations()
        {
            // Arrange
            var logger = new Mock<ILogger<DatabaseService>>();
            var service = new DatabaseService(_context!, logger.Object);

            // Act
            await service.InitializeAsync();

            // Assert
            var translations = await _context!.Translations.ToListAsync();
            Assert.NotEmpty(translations);
            Assert.All(translations, t =>
            {
                Assert.False(string.IsNullOrEmpty(t.Key));
                Assert.False(string.IsNullOrEmpty(t.Language));
                Assert.False(string.IsNullOrEmpty(t.Value));
            });
        }

        [Fact]
        public async Task InitializeAsync_SeedsMultipleLanguages()
        {
            // Arrange
            var logger = new Mock<ILogger<DatabaseService>>();
            var service = new DatabaseService(_context!, logger.Object);

            // Act
            await service.InitializeAsync();

            // Assert
            var languages = await _context!.Translations
                .Select(t => t.Language)
                .Distinct()
                .ToListAsync();
            
            Assert.Contains("de", languages);
            Assert.Contains("en", languages);
        }

        [Fact]
        public async Task InitializeAsync_DoesNotDuplicateDataOnMultipleCalls()
        {
            // Arrange
            var logger = new Mock<ILogger<DatabaseService>>();
            var service = new DatabaseService(_context!, logger.Object);

            // Act
            await service.InitializeAsync();
            var firstCount = await _context!.Wandersteine.CountAsync();
            
            await service.InitializeAsync();
            var secondCount = await _context.Wandersteine.CountAsync();

            // Assert
            Assert.Equal(firstCount, secondCount);
        }
    }
}
