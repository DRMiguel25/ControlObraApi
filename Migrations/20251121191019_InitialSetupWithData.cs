using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ControlObraApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetupWithData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Proyectos",
                columns: table => new
                {
                    ProyectoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreObra = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proyectos", x => x.ProyectoID);
                });

            migrationBuilder.CreateTable(
                name: "EstimacionesCosto",
                columns: table => new
                {
                    CostoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Concepto = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MontoEstimado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProyectoID = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstimacionesCosto", x => x.CostoID);
                    table.ForeignKey(
                        name: "FK_EstimacionesCosto_Proyectos_ProyectoID",
                        column: x => x.ProyectoID,
                        principalTable: "Proyectos",
                        principalColumn: "ProyectoID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AvancesObra",
                columns: table => new
                {
                    AvanceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MontoEjecutado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PorcentajeCompletado = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CostoID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvancesObra", x => x.AvanceID);
                    table.ForeignKey(
                        name: "FK_AvancesObra_EstimacionesCosto_CostoID",
                        column: x => x.CostoID,
                        principalTable: "EstimacionesCosto",
                        principalColumn: "CostoID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Proyectos",
                columns: new[] { "ProyectoID", "FechaInicio", "NombreObra", "Ubicacion" },
                values: new object[] { 2, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Torre Residencial Alpha", "Zona Central" });

            migrationBuilder.InsertData(
                table: "EstimacionesCosto",
                columns: new[] { "CostoID", "Concepto", "MontoEstimado", "ProyectoID" },
                values: new object[,]
                {
                    { 1, "Cimentación y Estructura", 150000.00m, 2 },
                    { 2, "Instalaciones Eléctricas", 45000.00m, 2 }
                });

            migrationBuilder.InsertData(
                table: "AvancesObra",
                columns: new[] { "AvanceID", "CostoID", "FechaRegistro", "MontoEjecutado", "PorcentajeCompletado" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 11, 11, 13, 10, 18, 649, DateTimeKind.Local).AddTicks(9882), 75000.00m, 50.00m },
                    { 2, 2, new DateTime(2025, 11, 16, 13, 10, 18, 649, DateTimeKind.Local).AddTicks(9892), 10000.00m, 20.00m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AvancesObra_CostoID",
                table: "AvancesObra",
                column: "CostoID");

            migrationBuilder.CreateIndex(
                name: "IX_EstimacionesCosto_ProyectoID",
                table: "EstimacionesCosto",
                column: "ProyectoID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvancesObra");

            migrationBuilder.DropTable(
                name: "EstimacionesCosto");

            migrationBuilder.DropTable(
                name: "Proyectos");
        }
    }
}
