using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class AlimentarTablaVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImageUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "", "Detalle de la villa....", new DateTime(2023, 4, 13, 7, 6, 58, 972, DateTimeKind.Local).AddTicks(7778), new DateTime(2023, 4, 13, 7, 6, 58, 972, DateTimeKind.Local).AddTicks(7765), "", 50, "Villa Real", 5, 200.0 },
                    { 2, "", "Detalle de la villa....", new DateTime(2023, 4, 13, 7, 6, 58, 972, DateTimeKind.Local).AddTicks(7781), new DateTime(2023, 4, 13, 7, 6, 58, 972, DateTimeKind.Local).AddTicks(7780), "", 40, "Premiun vista a la piscina", 4, 150.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
