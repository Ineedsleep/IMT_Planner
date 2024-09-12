using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMT_Planner_DAL.Migrations
{
    /// <inheritdoc />
    public partial class TagsForSM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "SuperManagers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tags",
                table: "SuperManagers");
        }
    }
}
