using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TrailmarksApi.Services;

namespace TrailmarksApi.Tests.Services
{
    public class DatabaseServiceTests
    {
        [Fact]
        public async Task InitializeAsync_ExecutesSuccessfully()
        {
            // Arrange
            var context = DatabaseFixture.CreateInMemoryContext();
            var logger = new Mock<ILogger<DatabaseService>>();
            var service = new DatabaseService(context, logger.Object);

            // Act
            await service.InitializeAsync();

            // Assert
            // Verify context has data seeded
            Assert.True(await context.Wandersteine.AnyAsync());
        }

        [Fact]
        public async Task InitializeAsync_SeedsWandersteine()
        {
            // Arrange
            var context = DatabaseFixture.CreateInMemoryContext();
            var logger = new Mock<ILogger<DatabaseService>>();
            var service = new DatabaseService(context, logger.Object);

            // Act
            await service.InitializeAsync();

            // Assert
            var wandersteine = await context.Wandersteine.ToListAsync();
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
            var context = DatabaseFixture.CreateInMemoryContext();
            var logger = new Mock<ILogger<DatabaseService>>();
            var service = new DatabaseService(context, logger.Object);

            // Act
            await service.InitializeAsync();

            // Assert
            var translations = await context.Translations.ToListAsync();
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
            var context = DatabaseFixture.CreateInMemoryContext();
            var logger = new Mock<ILogger<DatabaseService>>();
            var service = new DatabaseService(context, logger.Object);

            // Act
            await service.InitializeAsync();

            // Assert
            var languages = await context.Translations
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
            var context = DatabaseFixture.CreateInMemoryContext();
            var logger = new Mock<ILogger<DatabaseService>>();
            var service = new DatabaseService(context, logger.Object);

            // Act
            await service.InitializeAsync();
            var firstCount = await context.Wandersteine.CountAsync();
            
            await service.InitializeAsync();
            var secondCount = await context.Wandersteine.CountAsync();

            // Assert
            Assert.Equal(firstCount, secondCount);
        }
    }
}
