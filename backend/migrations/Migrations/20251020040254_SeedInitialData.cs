using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrailmarksApi.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var now = DateTime.UtcNow;
            
            // Seed Wandersteine data
            migrationBuilder.InsertData(
                table: "Wandersteine",
                columns: new[] { "Name", "unique_id", "preview_url", "Description", "Location", "Latitude", "Longitude", "created_at", "updated_at" },
                values: new object[,]
                {
                    // Germany - Black Forest cluster
                    { "Schwarzwaldstein", "WS-2024-001", "https://picsum.photos/300/200?random=1", "Ein historischer Wanderstein im Herzen des Schwarzwaldes", "Schwarzwald, Baden-Württemberg", 48.3019, 8.2392, now.AddDays(-20), now.AddDays(-20) },
                    { "Feldbergblick", "WS-2024-007", "https://picsum.photos/300/200?random=7", "Wanderstein am höchsten Punkt des Schwarzwaldes", "Feldberg, Baden-Württemberg", 47.8742, 8.0044, now.AddDays(-19), now.AddDays(-19) },
                    { "Titisee Rundweg", "WS-2024-008", "https://picsum.photos/300/200?random=8", "Malerischer Wanderstein am Titisee", "Titisee-Neustadt, Baden-Württemberg", 47.9034, 8.2064, now.AddDays(-18), now.AddDays(-18) },
                    { "Triberger Wasserfall", "WS-2024-009", "https://picsum.photos/300/200?random=9", "Wanderstein bei Deutschlands höchsten Wasserfällen", "Triberg, Baden-Württemberg", 48.1294, 8.2303, now.AddDays(-17), now.AddDays(-17) },
                    
                    // Germany - Rhine Valley cluster
                    { "Loreley Felsen", "WS-2024-010", "https://picsum.photos/300/200?random=10", "Legendärer Wanderstein am Rhein", "St. Goarshausen, Rheinland-Pfalz", 50.1389, 7.7311, now.AddDays(-16), now.AddDays(-16) },
                    { "Burg Rheinfels", "WS-2024-011", "https://picsum.photos/300/200?random=11", "Wanderstein an historischer Burgruine", "St. Goar, Rheinland-Pfalz", 50.1503, 7.7142, now.AddDays(-15), now.AddDays(-15) },
                    { "Rheinsteig Aussicht", "WS-2024-012", "https://picsum.photos/300/200?random=12", "Panoramablick über das Rheintal", "Boppard, Rheinland-Pfalz", 50.2319, 7.5897, now.AddDays(-14), now.AddDays(-14) },
                    
                    // Germany - Bavarian Alps cluster
                    { "Alpenblick", "WS-2024-004", "https://picsum.photos/300/200?random=4", "Wanderstein auf dem höchsten Punkt der Route", "Allgäu, Bayern", 47.5596, 10.7498, now.AddDays(-13), now.AddDays(-13) },
                    { "Nebelhorn", "WS-2024-013", "https://picsum.photos/300/200?random=13", "Hochalpiner Wanderstein mit 400-Gipfel-Blick", "Oberstdorf, Bayern", 47.4119, 10.3233, now.AddDays(-12), now.AddDays(-12) },
                    { "Königssee Panorama", "WS-2024-014", "https://picsum.photos/300/200?random=14", "Wanderstein am smaragdgrünen Königssee", "Schönau am Königssee, Bayern", 47.5667, 12.9833, now.AddDays(-11), now.AddDays(-11) },
                    { "Watzmann Ostwand", "WS-2024-015", "https://picsum.photos/300/200?random=15", "Wanderstein mit Blick auf die berühmte Ostwand", "Berchtesgaden, Bayern", 47.5550, 12.9350, now.AddDays(-10), now.AddDays(-10) },
                    
                    // Germany - Harz cluster
                    { "Brocken Gipfel", "WS-2024-016", "https://picsum.photos/300/200?random=16", "Wanderstein auf dem höchsten Harzgipfel", "Wernigerode, Sachsen-Anhalt", 51.7992, 10.6147, now.AddDays(-9), now.AddDays(-9) },
                    { "Hexentanzplatz", "WS-2024-017", "https://picsum.photos/300/200?random=17", "Mystischer Wanderstein an sagenhaftem Ort", "Thale, Sachsen-Anhalt", 51.7503, 11.0308, now.AddDays(-8), now.AddDays(-8) },
                    { "Rappbodetalsperre", "WS-2024-018", "https://picsum.photos/300/200?random=18", "Wanderstein an der größten Talsperre im Harz", "Oberharz am Brocken, Sachsen-Anhalt", 51.7489, 10.9044, now.AddDays(-7), now.AddDays(-7) },
                    
                    // International stones
                    { "Rocky Mountain Summit", "WS-2024-002", "https://picsum.photos/300/200?random=2", "Wanderstein mit herrlichem Blick auf die Rocky Mountains", "Colorado, USA", 39.7392, -104.9903, now.AddDays(-6), now.AddDays(-6) },
                    { "Mount Fuji Trail", "WS-2024-003", "https://picsum.photos/300/200?random=3", "Markanter Stein auf dem Weg zum Mount Fuji", "Fujinomiya, Japan", 35.3606, 138.7278, now.AddDays(-5), now.AddDays(-5) },
                    { "Outback Stone", "WS-2024-005", "https://picsum.photos/300/200?random=5", "Ruhiger Wanderstein im australischen Outback", "Uluru, Northern Territory, Australia", -25.3444, 131.0369, now.AddDays(-4), now.AddDays(-4) },
                    { "Patagonia Vista", "WS-2024-006", "https://picsum.photos/300/200?random=6", "Wanderstein mit Blick auf die patagonische Landschaft", "Torres del Paine, Chile", -51.2527, -72.9653, now.AddDays(-3), now.AddDays(-3) }
                });

            // Seed Translations data
            migrationBuilder.InsertData(
                table: "Translations",
                columns: new[] { "Key", "Language", "Value", "CreatedAt", "UpdatedAt" },
                values: new object[,]
                {
                    // German translations
                    { "common.loading", "de", "Lädt...", now, now },
                    { "common.error", "de", "Fehler", now, now },
                    { "common.retry", "de", "Erneut versuchen", now, now },
                    { "common.noData", "de", "Keine Daten gefunden", now, now },
                    { "header.language", "de", "Sprache", now, now },
                    { "wanderstein.title", "de", "Neueste Wandersteine", now, now },
                    { "wanderstein.subtitle", "de", "Die 5 zuletzt hinzugefügten Wandersteine", now, now },
                    { "wanderstein.loading", "de", "Lade Wandersteine...", now, now },
                    { "wanderstein.error", "de", "Fehler beim Laden der Wandersteine", now, now },
                    { "wanderstein.noData", "de", "Keine Wandersteine gefunden.", now, now },
                    { "wanderstein.addedOn", "de", "Hinzugefügt", now, now },
                    { "wanderstein.map.title", "de", "Kartenübersicht", now, now },
                    { "wanderstein.recent.title", "de", "Neueste Wandersteine", now, now },

                    // English translations
                    { "common.loading", "en", "Loading...", now, now },
                    { "common.error", "en", "Error", now, now },
                    { "common.retry", "en", "Retry", now, now },
                    { "common.noData", "en", "No data found", now, now },
                    { "header.language", "en", "Language", now, now },
                    { "wanderstein.title", "en", "Latest Hiking Stones", now, now },
                    { "wanderstein.subtitle", "en", "The 5 most recently added hiking stones", now, now },
                    { "wanderstein.loading", "en", "Loading hiking stones...", now, now },
                    { "wanderstein.error", "en", "Error loading hiking stones", now, now },
                    { "wanderstein.noData", "en", "No hiking stones found.", now, now },
                    { "wanderstein.addedOn", "en", "Added on", now, now },
                    { "wanderstein.map.title", "en", "Map Overview", now, now },
                    { "wanderstein.recent.title", "en", "Recent Hiking Stones", now, now }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Delete all seeded data
            migrationBuilder.Sql("DELETE FROM \"Wandersteine\" WHERE \"unique_id\" IN ('WS-2024-001', 'WS-2024-002', 'WS-2024-003', 'WS-2024-004', 'WS-2024-005', 'WS-2024-006', 'WS-2024-007', 'WS-2024-008', 'WS-2024-009', 'WS-2024-010', 'WS-2024-011', 'WS-2024-012', 'WS-2024-013', 'WS-2024-014', 'WS-2024-015', 'WS-2024-016', 'WS-2024-017', 'WS-2024-018')");
            migrationBuilder.Sql("DELETE FROM \"Translations\"");
        }
    }
}
