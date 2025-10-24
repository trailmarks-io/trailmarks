using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrailmarksApi.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddDetailPageTranslations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var now = new DateTime(2025, 10, 22, 19, 45, 0, DateTimeKind.Utc);
            
            // Add German translations for detail page
            migrationBuilder.InsertData(
                table: "Translations",
                columns: new[] { "Key", "Language", "Value", "CreatedAt", "UpdatedAt" },
                values: new object[,]
                {
                    { "wanderstein.detail.createdAt", "de", "Erstellt am", now, now },
                    { "wanderstein.detail.id", "de", "ID", now, now },
                    { "wanderstein.detail.location", "de", "Standortbeschreibung", now, now },
                    { "wanderstein.detail.map", "de", "Karte", now, now },
                    { "wanderstein.detail.coordinates", "de", "Koordinaten", now, now },
                    { "wanderstein.detail.noCoordinates", "de", "Für diesen Wanderstein sind keine Koordinaten verfügbar.", now, now },
                    { "wanderstein.detail.error", "de", "Fehler beim Laden der Details", now, now },
                    { "wanderstein.detail.error.noId", "de", "Keine Wanderstein-ID angegeben", now, now },
                    { "common.back", "de", "Zurück", now, now }
                });

            // Add English translations for detail page
            migrationBuilder.InsertData(
                table: "Translations",
                columns: new[] { "Key", "Language", "Value", "CreatedAt", "UpdatedAt" },
                values: new object[,]
                {
                    { "wanderstein.detail.createdAt", "en", "Created on", now, now },
                    { "wanderstein.detail.id", "en", "ID", now, now },
                    { "wanderstein.detail.location", "en", "Location Description", now, now },
                    { "wanderstein.detail.map", "en", "Map", now, now },
                    { "wanderstein.detail.coordinates", "en", "Coordinates", now, now },
                    { "wanderstein.detail.noCoordinates", "en", "No coordinates available for this hiking stone.", now, now },
                    { "wanderstein.detail.error", "en", "Error loading details", now, now },
                    { "wanderstein.detail.error.noId", "en", "No hiking stone ID provided", now, now },
                    { "common.back", "en", "Back", now, now }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove detail page translations
            migrationBuilder.Sql(@"
                DELETE FROM ""Translations"" 
                WHERE ""Key"" IN (
                    'wanderstein.detail.createdAt',
                    'wanderstein.detail.id',
                    'wanderstein.detail.location',
                    'wanderstein.detail.map',
                    'wanderstein.detail.coordinates',
                    'wanderstein.detail.noCoordinates',
                    'wanderstein.detail.error',
                    'wanderstein.detail.error.noId',
                    'common.back'
                )
            ");
        }
    }
}
