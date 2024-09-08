using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMT_Planner_DAL.Migrations
{
    /// <inheritdoc />
    public partial class Passives : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "HasShaftUnlockReduction",
                table: "SuperManagers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "ShaftUnlockReduction",
                table: "SuperManagers",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CRValue",
                table: "SuperManagers");

            migrationBuilder.DropColumn(
                name: "HasCR",
                table: "SuperManagers");

            migrationBuilder.DropColumn(
                name: "HasShaftUnlockReduction",
                table: "SuperManagers");

            migrationBuilder.DropColumn(
                name: "ShaftUnlockReduction",
                table: "SuperManagers");
        }
    }
}
