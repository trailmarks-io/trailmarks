using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrailmarksApi.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Seed Wandersteine
            var baseDate = new DateTime(2025, 10, 19, 21, 0, 0, DateTimeKind.Utc);
            
            migrationBuilder.InsertData(
                table: "Wandersteine",
                columns: new[] { "Name", "unique_id", "preview_url", "Description", "Location", "Latitude", "Longitude", "created_at", "updated_at" },
                values: new object[,]
                {
                    // Germany - Black Forest cluster
                    { "Schwarzwaldstein", "WS-2024-001", "https://picsum.photos/300/200?random=1", "Ein historischer Wanderstein im Herzen des Schwarzwaldes", "Schwarzwald, Baden-Württemberg", 48.3019, 8.2392, baseDate.AddDays(-20), baseDate.AddDays(-20) },
                    { "Feldbergblick", "WS-2024-007", "https://picsum.photos/300/200?random=7", "Wanderstein am höchsten Punkt des Schwarzwaldes", "Feldberg, Baden-Württemberg", 47.8742, 8.0044, baseDate.AddDays(-19), baseDate.AddDays(-19) },
                    { "Titisee Rundweg", "WS-2024-008", "https://picsum.photos/300/200?random=8", "Malerischer Wanderstein am Titisee", "Titisee-Neustadt, Baden-Württemberg", 47.9034, 8.2064, baseDate.AddDays(-18), baseDate.AddDays(-18) },
                    { "Triberger Wasserfall", "WS-2024-009", "https://picsum.photos/300/200?random=9", "Wanderstein bei Deutschlands höchsten Wasserfällen", "Triberg, Baden-Württemberg", 48.1294, 8.2303, baseDate.AddDays(-17), baseDate.AddDays(-17) },
                    
                    // Germany - Rhine Valley cluster
                    { "Loreley Felsen", "WS-2024-010", "https://picsum.photos/300/200?random=10", "Legendärer Wanderstein am Rhein", "St. Goarshausen, Rheinland-Pfalz", 50.1389, 7.7311, baseDate.AddDays(-16), baseDate.AddDays(-16) },
                    { "Burg Rheinfels", "WS-2024-011", "https://picsum.photos/300/200?random=11", "Wanderstein an historischer Burgruine", "St. Goar, Rheinland-Pfalz", 50.1503, 7.7142, baseDate.AddDays(-15), baseDate.AddDays(-15) },
                    { "Rheinsteig Aussicht", "WS-2024-012", "https://picsum.photos/300/200?random=12", "Panoramablick über das Rheintal", "Boppard, Rheinland-Pfalz", 50.2319, 7.5897, baseDate.AddDays(-14), baseDate.AddDays(-14) },
                    
                    // Germany - Bavarian Alps cluster
                    { "Alpenblick", "WS-2024-004", "https://picsum.photos/300/200?random=4", "Wanderstein auf dem höchsten Punkt der Route", "Allgäu, Bayern", 47.5596, 10.7498, baseDate.AddDays(-13), baseDate.AddDays(-13) },
                    { "Nebelhorn", "WS-2024-013", "https://picsum.photos/300/200?random=13", "Hochalpiner Wanderstein mit 400-Gipfel-Blick", "Oberstdorf, Bayern", 47.4119, 10.3233, baseDate.AddDays(-12), baseDate.AddDays(-12) },
                    { "Königssee Panorama", "WS-2024-014", "https://picsum.photos/300/200?random=14", "Wanderstein am smaragdgrünen Königssee", "Schönau am Königssee, Bayern", 47.5667, 12.9833, baseDate.AddDays(-11), baseDate.AddDays(-11) },
                    { "Watzmann Ostwand", "WS-2024-015", "https://picsum.photos/300/200?random=15", "Wanderstein mit Blick auf die berühmte Ostwand", "Berchtesgaden, Bayern", 47.5550, 12.9350, baseDate.AddDays(-10), baseDate.AddDays(-10) },
                    
                    // Germany - Harz cluster
                    { "Brocken Gipfel", "WS-2024-016", "https://picsum.photos/300/200?random=16", "Wanderstein auf dem höchsten Harzgipfel", "Wernigerode, Sachsen-Anhalt", 51.7992, 10.6147, baseDate.AddDays(-9), baseDate.AddDays(-9) },
                    { "Hexentanzplatz", "WS-2024-017", "https://picsum.photos/300/200?random=17", "Mystischer Wanderstein an sagenhaftem Ort", "Thale, Sachsen-Anhalt", 51.7503, 11.0308, baseDate.AddDays(-8), baseDate.AddDays(-8) },
                    { "Rappbodetalsperre", "WS-2024-018", "https://picsum.photos/300/200?random=18", "Wanderstein an der größten Talsperre im Harz", "Oberharz am Brocken, Sachsen-Anhalt", 51.7489, 10.9044, baseDate.AddDays(-7), baseDate.AddDays(-7) },
                    
                    // International stones
                    { "Rocky Mountain Summit", "WS-2024-002", "https://picsum.photos/300/200?random=2", "Wanderstein mit herrlichem Blick auf die Rocky Mountains", "Colorado, USA", 39.7392, -104.9903, baseDate.AddDays(-6), baseDate.AddDays(-6) },
                    { "Mount Fuji Trail", "WS-2024-003", "https://picsum.photos/300/200?random=3", "Markanter Stein auf dem Weg zum Mount Fuji", "Fujinomiya, Japan", 35.3606, 138.7278, baseDate.AddDays(-5), baseDate.AddDays(-5) },
                    { "Outback Stone", "WS-2024-005", "https://picsum.photos/300/200?random=5", "Ruhiger Wanderstein im australischen Outback", "Uluru, Northern Territory, Australia", -25.3444, 131.0369, baseDate.AddDays(-4), baseDate.AddDays(-4) },
                    { "Patagonia Vista", "WS-2024-006", "https://picsum.photos/300/200?random=6", "Wanderstein mit Blick auf die patagonische Landschaft", "Torres del Paine, Chile", -51.2527, -72.9653, baseDate.AddDays(-3), baseDate.AddDays(-3) }
                });

            // Seed Translations
            migrationBuilder.InsertData(
                table: "Translations",
                columns: new[] { "Key", "Language", "Value", "CreatedAt", "UpdatedAt" },
                values: new object[,]
                {
                    // German translations
                    { "common.loading", "de", "Lädt...", baseDate, baseDate },
                    { "common.error", "de", "Fehler", baseDate, baseDate },
                    { "common.retry", "de", "Erneut versuchen", baseDate, baseDate },
                    { "common.noData", "de", "Keine Daten gefunden", baseDate, baseDate },
                    { "header.language", "de", "Sprache", baseDate, baseDate },
                    { "wanderstein.title", "de", "Neueste Wandersteine", baseDate, baseDate },
                    { "wanderstein.subtitle", "de", "Die 5 zuletzt hinzugefügten Wandersteine", baseDate, baseDate },
                    { "wanderstein.loading", "de", "Lade Wandersteine...", baseDate, baseDate },
                    { "wanderstein.error", "de", "Fehler beim Laden der Wandersteine", baseDate, baseDate },
                    { "wanderstein.noData", "de", "Keine Wandersteine gefunden.", baseDate, baseDate },
                    { "wanderstein.addedOn", "de", "Hinzugefügt", baseDate, baseDate },
                    { "wanderstein.map.title", "de", "Kartenübersicht", baseDate, baseDate },
                    { "wanderstein.recent.title", "de", "Neueste Wandersteine", baseDate, baseDate },

                    // English translations
                    { "common.loading", "en", "Loading...", baseDate, baseDate },
                    { "common.error", "en", "Error", baseDate, baseDate },
                    { "common.retry", "en", "Retry", baseDate, baseDate },
                    { "common.noData", "en", "No data found", baseDate, baseDate },
                    { "header.language", "en", "Language", baseDate, baseDate },
                    { "wanderstein.title", "en", "Latest Hiking Stones", baseDate, baseDate },
                    { "wanderstein.subtitle", "en", "The 5 most recently added hiking stones", baseDate, baseDate },
                    { "wanderstein.loading", "en", "Loading hiking stones...", baseDate, baseDate },
                    { "wanderstein.error", "en", "Error loading hiking stones", baseDate, baseDate },
                    { "wanderstein.noData", "en", "No hiking stones found.", baseDate, baseDate },
                    { "wanderstein.addedOn", "en", "Added on", baseDate, baseDate },
                    { "wanderstein.map.title", "en", "Map Overview", baseDate, baseDate },
                    { "wanderstein.recent.title", "en", "Recent Hiking Stones", baseDate, baseDate }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Delete all seed data
            migrationBuilder.DeleteData(
                table: "Wandersteine",
                keyColumn: "unique_id",
                keyValues: new object[]
                {
                    "WS-2024-001", "WS-2024-002", "WS-2024-003", "WS-2024-004", "WS-2024-005",
                    "WS-2024-006", "WS-2024-007", "WS-2024-008", "WS-2024-009", "WS-2024-010",
                    "WS-2024-011", "WS-2024-012", "WS-2024-013", "WS-2024-014", "WS-2024-015",
                    "WS-2024-016", "WS-2024-017", "WS-2024-018"
                });

            migrationBuilder.DeleteData(
                table: "Translations",
                keyColumn: "Key",
                keyValues: new object[]
                {
                    "common.loading", "common.error", "common.retry", "common.noData",
                    "header.language", "wanderstein.title", "wanderstein.subtitle",
                    "wanderstein.loading", "wanderstein.error", "wanderstein.noData",
                    "wanderstein.addedOn", "wanderstein.map.title", "wanderstein.recent.title"
                });
        }
    }
}
