using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarTablaVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Villas",
                newName: "ImagenUrl");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 4, 14, 6, 57, 21, 928, DateTimeKind.Local).AddTicks(5917), new DateTime(2023, 4, 14, 6, 57, 21, 928, DateTimeKind.Local).AddTicks(5902) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 4, 14, 6, 57, 21, 928, DateTimeKind.Local).AddTicks(5920), new DateTime(2023, 4, 14, 6, 57, 21, 928, DateTimeKind.Local).AddTicks(5919) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagenUrl",
                table: "Villas",
                newName: "ImageUrl");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 4, 13, 7, 6, 58, 972, DateTimeKind.Local).AddTicks(7778), new DateTime(2023, 4, 13, 7, 6, 58, 972, DateTimeKind.Local).AddTicks(7765) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 4, 13, 7, 6, 58, 972, DateTimeKind.Local).AddTicks(7781), new DateTime(2023, 4, 13, 7, 6, 58, 972, DateTimeKind.Local).AddTicks(7780) });
        }
    }
}
