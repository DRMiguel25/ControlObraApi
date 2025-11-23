using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlObraApi.Migrations
{
    /// <inheritdoc />
    public partial class AddNameAndEmailToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AvancesObra",
                keyColumn: "AvanceID",
                keyValue: 1,
                column: "FechaRegistro",
                value: new DateTime(2025, 11, 13, 0, 37, 27, 715, DateTimeKind.Local).AddTicks(5176));

            migrationBuilder.UpdateData(
                table: "AvancesObra",
                keyColumn: "AvanceID",
                keyValue: 2,
                column: "FechaRegistro",
                value: new DateTime(2025, 11, 18, 0, 37, 27, 715, DateTimeKind.Local).AddTicks(5184));

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Users");

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
    }
}
