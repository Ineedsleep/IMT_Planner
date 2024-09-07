using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMT_Planner_DAL.Migrations
{
    /// <inheritdoc />
    public partial class normalizepassives : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CRValue",
                table: "SuperManagers");

            migrationBuilder.DropColumn(
                name: "HasCR",
                table: "SuperManagers");

            migrationBuilder.DropColumn(
                name: "HasMultiplier",
                table: "SuperManagers");

            migrationBuilder.DropColumn(
                name: "HasShaftUnlockReduction",
                table: "SuperManagers");

            migrationBuilder.DropColumn(
                name: "PassiveMultiplier",
                table: "SuperManagers");

            migrationBuilder.DropColumn(
                name: "ShaftUnlockReduction",
                table: "SuperManagers");

            migrationBuilder.CreateTable(
                name: "Passives",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SuperManagerId = table.Column<int>(type: "INTEGER", nullable: false),
                    HasMif = table.Column<bool>(type: "INTEGER", nullable: false),
                    MineIncomeFactor = table.Column<double>(type: "REAL", nullable: false),
                    HasCif = table.Column<bool>(type: "INTEGER", nullable: false),
                    ContinentIncomeFactor = table.Column<double>(type: "REAL", nullable: true),
                    HasCostReduction = table.Column<bool>(type: "INTEGER", nullable: false),
                    CostReduction = table.Column<double>(type: "REAL", nullable: true),
                    HasShaftUnlockReduction = table.Column<bool>(type: "INTEGER", nullable: false),
                    ShaftUnlockReduction = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Passives_SuperManagers_SuperManagerId",
                        column: x => x.SuperManagerId,
                        principalTable: "SuperManagers",
                        principalColumn: "SuperManagerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Passives_SuperManagerId",
                table: "Passives",
                column: "SuperManagerId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Passives");

            migrationBuilder.AddColumn<double>(
                name: "CRValue",
                table: "SuperManagers",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "HasCR",
                table: "SuperManagers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasMultiplier",
                table: "SuperManagers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasShaftUnlockReduction",
                table: "SuperManagers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "PassiveMultiplier",
                table: "SuperManagers",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ShaftUnlockReduction",
                table: "SuperManagers",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
