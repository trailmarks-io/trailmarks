using System.ComponentModel.DataAnnotations;

namespace TrailmarksApi.Models
{
    /// <summary>
    /// Translation entity for storing UI text in multiple languages
    /// </summary>
    public class Translation
    {
        /// <summary>
        /// Unique identifier for the translation
        /// </summary>
        public uint Id { get; set; }

        /// <summary>
        /// Translation key (e.g., "wanderstein.title")
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// Language code (e.g., "de", "en")
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string Language { get; set; } = string.Empty;

        /// <summary>
        /// Translated text value
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// Creation timestamp
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Last update timestamp
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
