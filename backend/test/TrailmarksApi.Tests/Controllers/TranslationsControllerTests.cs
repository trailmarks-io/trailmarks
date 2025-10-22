using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TrailmarksApi.Controllers;
using TrailmarksApi.Data;
using TrailmarksApi.Models;

namespace TrailmarksApi.Tests.Controllers
{
    public class TranslationsControllerTests : IAsyncLifetime
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
        public async Task GetTranslations_ReturnsOkResultWithTranslations()
        {
            // Arrange
            _context!.Translations.Add(new Translation
            {
                Key = "app.title",
                Language = "de",
                Value = "Trailmarks"
            });
            await _context.SaveChangesAsync();

            var logger = new Mock<ILogger<TranslationsController>>();
            var controller = new TranslationsController(_context, logger.Object);

            // Act
            var result = await controller.GetTranslations("de");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task GetTranslations_ReturnsNotFoundForInvalidLanguage()
        {
            // Arrange
            var logger = new Mock<ILogger<TranslationsController>>();
            var controller = new TranslationsController(_context!, logger.Object);

            // Act
            var result = await controller.GetTranslations("fr");

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetTranslations_BuildsNestedDictionary()
        {
            // Arrange
            // Clear existing seed data for this test
            _context!.Translations.RemoveRange(_context.Translations);
            await _context.SaveChangesAsync();

            _context.Translations.AddRange(new List<Translation>
            {
                new Translation { Key = "app.title", Language = "en", Value = "Trailmarks" },
                new Translation { Key = "app.subtitle", Language = "en", Value = "Hiking Stones" },
                new Translation { Key = "wanderstein.error", Language = "en", Value = "Error loading stones" }
            });
            await _context.SaveChangesAsync();

            var logger = new Mock<ILogger<TranslationsController>>();
            var controller = new TranslationsController(_context, logger.Object);

            // Act
            var result = await controller.GetTranslations("en");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var dict = Assert.IsAssignableFrom<Dictionary<string, object>>(okResult.Value);
            Assert.True(dict.ContainsKey("app"));
            Assert.True(dict.ContainsKey("wanderstein"));
        }

        [Fact]
        public async Task GetTranslations_IsCaseInsensitive()
        {
            // Arrange
            _context!.Translations.Add(new Translation
            {
                Key = "test.key",
                Language = "de",
                Value = "Test"
            });
            await _context.SaveChangesAsync();

            var logger = new Mock<ILogger<TranslationsController>>();
            var controller = new TranslationsController(_context, logger.Object);

            // Act
            var result = await controller.GetTranslations("DE");

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetSupportedLanguages_ReturnsOkResult()
        {
            // Arrange
            var logger = new Mock<ILogger<TranslationsController>>();
            var controller = new TranslationsController(_context!, logger.Object);

            // Act
            var result = await controller.GetSupportedLanguages();

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetSupportedLanguages_ReturnsDistinctLanguages()
        {
            // Arrange
            _context!.Translations.AddRange(new List<Translation>
            {
                new Translation { Key = "key1", Language = "de", Value = "Wert1" },
                new Translation { Key = "key2", Language = "de", Value = "Wert2" },
                new Translation { Key = "key1", Language = "en", Value = "Value1" },
                new Translation { Key = "key3", Language = "fr", Value = "Valeur3" }
            });
            await _context.SaveChangesAsync();

            var logger = new Mock<ILogger<TranslationsController>>();
            var controller = new TranslationsController(_context, logger.Object);

            // Act
            var result = await controller.GetSupportedLanguages();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var languages = Assert.IsAssignableFrom<List<string>>(okResult.Value);
            Assert.Equal(3, languages.Count);
            Assert.Contains("de", languages);
            Assert.Contains("en", languages);
            Assert.Contains("fr", languages);
        }

        [Fact]
        public async Task GetSupportedLanguages_ReturnsEmptyListWhenNoTranslations()
        {
            // Arrange
            // Clear existing seed data for this test
            _context!.Translations.RemoveRange(_context.Translations);
            await _context.SaveChangesAsync();

            var logger = new Mock<ILogger<TranslationsController>>();
            var controller = new TranslationsController(_context, logger.Object);

            // Act
            var result = await controller.GetSupportedLanguages();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var languages = Assert.IsAssignableFrom<List<string>>(okResult.Value);
            Assert.Empty(languages);
        }

        [Fact]
        public async Task GetSupportedLanguages_ReturnsLanguagesInOrder()
        {
            // Arrange
            _context!.Translations.AddRange(new List<Translation>
            {
                new Translation { Key = "key1", Language = "fr", Value = "Val" },
                new Translation { Key = "key2", Language = "de", Value = "Val" },
                new Translation { Key = "key3", Language = "en", Value = "Val" }
            });
            await _context.SaveChangesAsync();

            var logger = new Mock<ILogger<TranslationsController>>();
            var controller = new TranslationsController(_context, logger.Object);

            // Act
            var result = await controller.GetSupportedLanguages();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var languages = Assert.IsAssignableFrom<List<string>>(okResult.Value);
            Assert.Equal(new List<string> { "de", "en", "fr" }, languages);
        }
    }
}
