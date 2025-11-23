using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlObraApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AvancesObra",
                keyColumn: "AvanceID",
                keyValue: 1,
                column: "FechaRegistro",
                value: new DateTime(2025, 11, 12, 11, 27, 13, 438, DateTimeKind.Local).AddTicks(6998));

            migrationBuilder.UpdateData(
                table: "AvancesObra",
                keyColumn: "AvanceID",
                keyValue: 2,
                column: "FechaRegistro",
                value: new DateTime(2025, 11, 17, 11, 27, 13, 438, DateTimeKind.Local).AddTicks(7003));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.UpdateData(
                table: "AvancesObra",
                keyColumn: "AvanceID",
                keyValue: 1,
                column: "FechaRegistro",
                value: new DateTime(2025, 11, 11, 13, 10, 18, 649, DateTimeKind.Local).AddTicks(9882));

            migrationBuilder.UpdateData(
                table: "AvancesObra",
                keyColumn: "AvanceID",
                keyValue: 2,
                column: "FechaRegistro",
                value: new DateTime(2025, 11, 16, 13, 10, 18, 649, DateTimeKind.Local).AddTicks(9892));
        }
    }
}
