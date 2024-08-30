using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IMT_Planner_DAL.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Elements",
                columns: table => new
                {
                    ElementId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elements", x => x.ElementId);
                });

            migrationBuilder.CreateTable(
                name: "SuperManagers",
                columns: table => new
                {
                    SuperManagerId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Rank = table.Column<int>(type: "INTEGER", nullable: true),
                    Promoted = table.Column<bool>(type: "INTEGER", nullable: false),
                    Level = table.Column<byte>(type: "INTEGER", nullable: false),
                    CurrentFragments = table.Column<int>(type: "INTEGER", nullable: false),
                    Equipment = table.Column<string>(type: "TEXT", nullable: false),
                    PassiveMultiplier = table.Column<double>(type: "REAL", nullable: false),
                    Rarity = table.Column<string>(type: "TEXT", nullable: false),
                    Area = table.Column<string>(type: "TEXT", nullable: false),
                    Priority = table.Column<byte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuperManagers", x => x.SuperManagerId);
                });

            migrationBuilder.CreateTable(
                name: "SuperManagerElements",
                columns: table => new
                {
                    SuperManagerId = table.Column<int>(type: "INTEGER", nullable: false),
                    ElementId = table.Column<int>(type: "INTEGER", nullable: false),
                    EffectivenessType = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuperManagerElements", x => new { x.SuperManagerId, x.ElementId });
                    table.ForeignKey(
                        name: "FK_SuperManagerElements_Elements_ElementId",
                        column: x => x.ElementId,
                        principalTable: "Elements",
                        principalColumn: "ElementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SuperManagerElements_SuperManagers_SuperManagerId",
                        column: x => x.SuperManagerId,
                        principalTable: "SuperManagers",
                        principalColumn: "SuperManagerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Elements",
                columns: new[] { "ElementId", "Name" },
                values: new object[,]
                {
                    { 1, "Nature" },
                    { 2, "Frost" },
                    { 3, "Flame" },
                    { 4, "Light" },
                    { 5, "Dark" },
                    { 6, "Wind" },
                    { 7, "Sand" },
                    { 8, "Water" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SuperManagerElements_ElementId",
                table: "SuperManagerElements",
                column: "ElementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SuperManagerElements");

            migrationBuilder.DropTable(
                name: "Elements");

            migrationBuilder.DropTable(
                name: "SuperManagers");
        }
    }
}
