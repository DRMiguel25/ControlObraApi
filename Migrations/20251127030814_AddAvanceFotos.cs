using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlObraApi.Migrations
{
    /// <inheritdoc />
    public partial class AddAvanceFotos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AvanceFotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Orientacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvanceObraId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvanceFotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvanceFotos_AvancesObra_AvanceObraId",
                        column: x => x.AvanceObraId,
                        principalTable: "AvancesObra",
                        principalColumn: "AvanceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AvanceFotos_AvanceObraId",
                table: "AvanceFotos",
                column: "AvanceObraId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvanceFotos");
        }
    }
}
