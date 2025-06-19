using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GimManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class Reportes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reportes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FechaGenerado = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FechaDesde = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FechaHasta = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TotalProductos = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalMembresias = table.Column<int>(type: "INTEGER", nullable: false),
                    IngresoProductos = table.Column<decimal>(type: "TEXT", nullable: false),
                    IngresoMembresias = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reportes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reportes");
        }
    }
}
