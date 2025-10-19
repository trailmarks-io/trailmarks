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
            var context = await DatabaseFixture.CreatePostgreSqlContextAsync();
            var logger = new Mock<ILogger<DatabaseService>>();
            var service = new DatabaseService(context, logger.Object);

            // Act
            await service.InitializeAsync();

            // Assert
            // Verify context has data seeded
            Assert.True(await context.Wandersteine.AnyAsync());
            
            // Cleanup
            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async Task InitializeAsync_SeedsWandersteine()
        {
            // Arrange
            var context = await DatabaseFixture.CreatePostgreSqlContextAsync();
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
            
            // Cleanup
            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async Task InitializeAsync_SeedsTranslations()
        {
            // Arrange
            var context = await DatabaseFixture.CreatePostgreSqlContextAsync();
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
            
            // Cleanup
            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async Task InitializeAsync_SeedsMultipleLanguages()
        {
            // Arrange
            var context = await DatabaseFixture.CreatePostgreSqlContextAsync();
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
            
            // Cleanup
            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }

        [Fact]
        public async Task InitializeAsync_DoesNotDuplicateDataOnMultipleCalls()
        {
            // Arrange
            var context = await DatabaseFixture.CreatePostgreSqlContextAsync();
            var logger = new Mock<ILogger<DatabaseService>>();
            var service = new DatabaseService(context, logger.Object);

            // Act
            await service.InitializeAsync();
            var firstCount = await context.Wandersteine.CountAsync();
            
            await service.InitializeAsync();
            var secondCount = await context.Wandersteine.CountAsync();

            // Assert
            Assert.Equal(firstCount, secondCount);
            
            // Cleanup
            await context.Database.EnsureDeletedAsync();
            await context.DisposeAsync();
        }
    }
}
