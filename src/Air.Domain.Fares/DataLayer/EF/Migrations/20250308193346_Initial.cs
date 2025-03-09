using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Air.Domain.Fares.DataLayer.EF.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlightFares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Airline = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    Origin = table.Column<int>(type: "int", nullable: false),
                    Destination = table.Column<int>(type: "int", nullable: false),
                    Fare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FlightNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartureUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArrivalUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AirId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightFares", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FlightFares_AirId",
                table: "FlightFares",
                column: "AirId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlightFares");
        }
    }
}
