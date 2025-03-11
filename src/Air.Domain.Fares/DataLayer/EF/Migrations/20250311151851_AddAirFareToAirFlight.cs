using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Air.Domain.Fares.DataLayer.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddAirFareToAirFlight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlightFares");

            migrationBuilder.CreateTable(
                name: "AirFlights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Airline = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Origin = table.Column<int>(type: "int", nullable: false),
                    Destination = table.Column<int>(type: "int", nullable: false),
                    FlightNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DepartureUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArrivalUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirFlights", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AirFares",
                columns: table => new
                {
                    FlightFareId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    Fare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastObservedUtc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Source = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AirFlightId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirFares", x => x.FlightFareId);
                    table.ForeignKey(
                        name: "FK_AirFares_AirFlights_AirFlightId",
                        column: x => x.AirFlightId,
                        principalTable: "AirFlights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AirFares_AirFlightId",
                table: "AirFares",
                column: "AirFlightId");

            migrationBuilder.CreateIndex(
                name: "IX_AirFares_Source",
                table: "AirFares",
                column: "Source");

            migrationBuilder.CreateIndex(
                name: "IX_AirFlights_FlightNumber_DepartureUtc",
                table: "AirFlights",
                columns: new[] { "FlightNumber", "DepartureUtc" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirFares");

            migrationBuilder.DropTable(
                name: "AirFlights");

            migrationBuilder.CreateTable(
                name: "FlightFares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AirId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Airline = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArrivalUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    DepartureUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Destination = table.Column<int>(type: "int", nullable: false),
                    Fare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FlightNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Origin = table.Column<int>(type: "int", nullable: false)
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
    }
}
