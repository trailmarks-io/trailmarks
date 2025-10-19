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

            // Act & Assert - should not throw
            await service.InitializeAsync();
            
            // Verify migrations were applied and data exists
            Assert.True(await context.Wandersteine.AnyAsync());
        }

        [Fact]
        public async Task Migrations_SeedWandersteine()
        {
            // Arrange & Act - migrations are applied in CreateInMemoryContext
            var context = DatabaseFixture.CreateInMemoryContext();

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
        public async Task Migrations_SeedTranslations()
        {
            // Arrange & Act - migrations are applied in CreateInMemoryContext
            var context = DatabaseFixture.CreateInMemoryContext();

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
        public async Task Migrations_SeedMultipleLanguages()
        {
            // Arrange & Act - migrations are applied in CreateInMemoryContext
            var context = DatabaseFixture.CreateInMemoryContext();

            // Assert
            var languages = await context.Translations
                .Select(t => t.Language)
                .Distinct()
                .ToListAsync();
            
            Assert.Contains("de", languages);
            Assert.Contains("en", languages);
        }

        [Fact]
        public async Task InitializeAsync_DoesNotFailOnMultipleCalls()
        {
            // Arrange
            var context = DatabaseFixture.CreateInMemoryContext();
            var logger = new Mock<ILogger<DatabaseService>>();
            var service = new DatabaseService(context, logger.Object);

            // Act - calling InitializeAsync multiple times should not fail
            await service.InitializeAsync();
            var firstCount = await context.Wandersteine.CountAsync();
            
            await service.InitializeAsync();
            var secondCount = await context.Wandersteine.CountAsync();

            // Assert - migrations are idempotent, so count should stay the same
            Assert.Equal(firstCount, secondCount);
        }
    }
}
