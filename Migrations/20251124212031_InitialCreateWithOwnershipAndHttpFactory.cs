using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ControlObraApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateWithOwnershipAndHttpFactory : Migration
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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proyectos",
                columns: table => new
                {
                    ProyectoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreObra = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proyectos", x => x.ProyectoID);
                    table.ForeignKey(
                        name: "FK_Proyectos_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash", "Role", "Username" },
                values: new object[] { 1, "demo@test.com", "Usuario Demo", "$2a$11$vY8y8HZg5LqW3z9X2wK7P.7TqK9Z3yN4WqR8P.XmF5Kx9V7Qz8z9e", "User", "demo" });

            migrationBuilder.InsertData(
                table: "Proyectos",
                columns: new[] { "ProyectoID", "FechaInicio", "NombreObra", "Ubicacion", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Torre Residencial Alpha", "Zona Central", 1 },
                    { 2, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Edificio Comercial Beta", "Zona Norte", 1 }
                });

            migrationBuilder.InsertData(
                table: "EstimacionesCosto",
                columns: new[] { "CostoID", "Concepto", "MontoEstimado", "ProyectoID" },
                values: new object[,]
                {
                    { 1, "Cimentación y Estructura", 150000.00m, 1 },
                    { 2, "Instalaciones Eléctricas", 45000.00m, 1 },
                    { 3, "Acabados Interiores", 80000.00m, 2 }
                });

            migrationBuilder.InsertData(
                table: "AvancesObra",
                columns: new[] { "AvanceID", "CostoID", "FechaRegistro", "MontoEjecutado", "PorcentajeCompletado" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 75000.00m, 50.00m },
                    { 2, 2, new DateTime(2025, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 10000.00m, 22.22m },
                    { 3, 3, new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 30000.00m, 37.50m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AvancesObra_CostoID",
                table: "AvancesObra",
                column: "CostoID");

            migrationBuilder.CreateIndex(
                name: "IX_EstimacionesCosto_ProyectoID",
                table: "EstimacionesCosto",
                column: "ProyectoID");

            migrationBuilder.CreateIndex(
                name: "IX_Proyectos_UserId",
                table: "Proyectos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
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

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
