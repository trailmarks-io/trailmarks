using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrailmarksApi.Data;
using TrailmarksApi.Models;

namespace TrailmarksApi.Controllers
{
    /// <summary>
    /// Controller for managing translations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TranslationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TranslationsController> _logger;

        public TranslationsController(ApplicationDbContext context, ILogger<TranslationsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Get all translations for a specific language
        /// </summary>
        /// <param name="language">Language code (e.g., "de", "en")</param>
        /// <returns>Dictionary of translation keys and values</returns>
        [HttpGet("{language}")]
        [ProducesResponseType(typeof(Dictionary<string, object>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTranslations(string language)
        {
            try
            {
                var translations = await _context.Translations
                    .Where(t => t.Language == language.ToLower())
                    .ToListAsync();

                if (!translations.Any())
                {
                    return NotFound(new { message = $"No translations found for language '{language}'" });
                }

                // Build nested dictionary structure from flat keys
                var result = new Dictionary<string, object>();
                foreach (var translation in translations)
                {
                    SetNestedValue(result, translation.Key, translation.Value);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching translations for language {Language}", language);
                return Problem(
                    title: "Error fetching translations",
                    statusCode: 500,
                    detail: "An error occurred while retrieving translations"
                );
            }
        }

        /// <summary>
        /// Get all supported languages
        /// </summary>
        /// <returns>List of language codes</returns>
        [HttpGet("languages")]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSupportedLanguages()
        {
            try
            {
                var languages = await _context.Translations
                    .Select(t => t.Language)
                    .Distinct()
                    .OrderBy(l => l)
                    .ToListAsync();

                return Ok(languages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching supported languages");
                return Problem(
                    title: "Error fetching languages",
                    statusCode: 500,
                    detail: "An error occurred while retrieving supported languages"
                );
            }
        }

        /// <summary>
        /// Helper method to set nested dictionary values from dot-notation keys
        /// </summary>
        private void SetNestedValue(Dictionary<string, object> dict, string key, string value)
        {
            var parts = key.Split('.');
            Dictionary<string, object> current = dict;

            for (int i = 0; i < parts.Length - 1; i++)
            {
                if (!current.ContainsKey(parts[i]))
                {
                    current[parts[i]] = new Dictionary<string, object>();
                }
                else if (current[parts[i]] is not Dictionary<string, object>)
                {
                    // If the current value is not a dictionary (e.g., it's a string),
                    // we can't navigate deeper. This can happen if we have keys like:
                    // "a.b.c" = "value1" and "a.b.c.d" = "value2"
                    // In this case, we skip the nested value to preserve the first one.
                    return;
                }
                current = (Dictionary<string, object>)current[parts[i]];
            }

            current[parts[^1]] = value;
        }
    }
}
