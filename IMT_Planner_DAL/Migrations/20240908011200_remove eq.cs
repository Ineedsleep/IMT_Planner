using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMT_Planner_DAL.Migrations
{
    /// <inheritdoc />
    public partial class removeeq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Equipment",
                table: "SuperManagers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Equipment",
                table: "SuperManagers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
