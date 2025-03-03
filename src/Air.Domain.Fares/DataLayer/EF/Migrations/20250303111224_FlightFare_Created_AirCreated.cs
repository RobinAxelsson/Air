using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Air.Domain.Fares.DataLayer.EF.Migrations
{
    /// <inheritdoc />
    public partial class FlightFare_Created_AirCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Created",
                table: "FlightFares",
                newName: "AirCreated");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AirCreated",
                table: "FlightFares",
                newName: "Created");
        }
    }
}
