using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMT_Planner_DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddMifbool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasMultiplier",
                table: "SuperManagers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasMultiplier",
                table: "SuperManagers");
        }
    }
}
