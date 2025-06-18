using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GimManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class TablaEmpleados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ApellidoPaterno = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ApellidoMaterno = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Sueldo = table.Column<decimal>(type: "TEXT", nullable: false),
                    Rol = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    FechaIngreso = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Empleados");
        }
    }
}
