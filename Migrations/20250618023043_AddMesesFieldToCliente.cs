using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GimManager.Migrations
{
    /// <inheritdoc />
    public partial class AddMesesFieldToCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaVencimiento",
                table: "Clientes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "PrecioMembresia",
                table: "Clientes",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaVencimiento",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "PrecioMembresia",
                table: "Clientes");
        }
    }
}
