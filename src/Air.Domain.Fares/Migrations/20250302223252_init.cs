using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Air.Domain;

/// <inheritdoc />
public partial class init : Migration
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
                Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Origin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Destination = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Fare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                FlightNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Departure = table.Column<DateTime>(type: "datetime2", nullable: false),
                Arrival = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_FlightFares", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "FlightFares");
    }
}
