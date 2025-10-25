using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace TrailmarksApi.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class ConvertCoordinatesToPointColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add the new Location_Point column
            migrationBuilder.AddColumn<Point>(
                name: "Location_Point",
                table: "Wandersteine",
                type: "geography (point, 4326)",
                nullable: true);

            // Migrate existing Latitude/Longitude data to Location_Point
            // Use PostGIS ST_SetSRID and ST_MakePoint to create geography points
            migrationBuilder.Sql(@"
                UPDATE ""Wandersteine""
                SET ""Location_Point"" = ST_SetSRID(ST_MakePoint(""Longitude"", ""Latitude""), 4326)::geography
                WHERE ""Latitude"" IS NOT NULL AND ""Longitude"" IS NOT NULL;
            ");

            // Drop the old Latitude and Longitude columns
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Wandersteine");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Wandersteine");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Add back Latitude and Longitude columns
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Wandersteine",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Wandersteine",
                type: "double precision",
                nullable: true);

            // Migrate Location_Point data back to Latitude/Longitude
            migrationBuilder.Sql(@"
                UPDATE ""Wandersteine""
                SET ""Latitude"" = ST_Y(""Location_Point""::geometry),
                    ""Longitude"" = ST_X(""Location_Point""::geometry)
                WHERE ""Location_Point"" IS NOT NULL;
            ");

            // Drop the Location_Point column
            migrationBuilder.DropColumn(
                name: "Location_Point",
                table: "Wandersteine");
        }
    }
}
