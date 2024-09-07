using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMT_Planner_DAL.Migrations
{
    /// <inheritdoc />
    public partial class passivesTopassive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Passives_SuperManagerId",
                table: "Passives");

            migrationBuilder.DropColumn(
                name: "ContinentIncomeFactor",
                table: "Passives");

            migrationBuilder.DropColumn(
                name: "CostReduction",
                table: "Passives");

            migrationBuilder.DropColumn(
                name: "HasCif",
                table: "Passives");

            migrationBuilder.DropColumn(
                name: "HasCostReduction",
                table: "Passives");

            migrationBuilder.DropColumn(
                name: "HasMif",
                table: "Passives");

            migrationBuilder.DropColumn(
                name: "MineIncomeFactor",
                table: "Passives");

            migrationBuilder.RenameColumn(
                name: "ShaftUnlockReduction",
                table: "Passives",
                newName: "AttributeValue");

            migrationBuilder.RenameColumn(
                name: "HasShaftUnlockReduction",
                table: "Passives",
                newName: "RankRequirement");

            migrationBuilder.AddColumn<string>(
                name: "AttributeName",
                table: "Passives",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Passives_SuperManagerId",
                table: "Passives",
                column: "SuperManagerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Passives_SuperManagerId",
                table: "Passives");

            migrationBuilder.DropColumn(
                name: "AttributeName",
                table: "Passives");

            migrationBuilder.RenameColumn(
                name: "RankRequirement",
                table: "Passives",
                newName: "HasShaftUnlockReduction");

            migrationBuilder.RenameColumn(
                name: "AttributeValue",
                table: "Passives",
                newName: "ShaftUnlockReduction");

            migrationBuilder.AddColumn<double>(
                name: "ContinentIncomeFactor",
                table: "Passives",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CostReduction",
                table: "Passives",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasCif",
                table: "Passives",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasCostReduction",
                table: "Passives",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasMif",
                table: "Passives",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "MineIncomeFactor",
                table: "Passives",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_Passives_SuperManagerId",
                table: "Passives",
                column: "SuperManagerId",
                unique: true);
        }
    }
}
