using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IMT_Planner_DAL.Migrations
{
    /// <inheritdoc />
    public partial class changepassivename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttributeName",
                table: "Passives");

            migrationBuilder.AddColumn<int>(
                name: "PassiveAttributeNameId",
                table: "Passives",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PassiveAttributeNames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Abbreviation = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassiveAttributeNames", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "PassiveAttributeNames",
                columns: new[] { "Id", "Abbreviation", "Description" },
                values: new object[,]
                {
                    { 1, "MIF", "Mine Income Factor" },
                    { 2, "CIF", "Continental Income Factor" },
                    { 3, "CR", "Cost reduction for current shaft lvl" },
                    { 4, "SUCR", "Shaft unlock cost reduction" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Passives_PassiveAttributeNameId",
                table: "Passives",
                column: "PassiveAttributeNameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Passives_PassiveAttributeNames_PassiveAttributeNameId",
                table: "Passives",
                column: "PassiveAttributeNameId",
                principalTable: "PassiveAttributeNames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passives_PassiveAttributeNames_PassiveAttributeNameId",
                table: "Passives");

            migrationBuilder.DropTable(
                name: "PassiveAttributeNames");

            migrationBuilder.DropIndex(
                name: "IX_Passives_PassiveAttributeNameId",
                table: "Passives");

            migrationBuilder.DropColumn(
                name: "PassiveAttributeNameId",
                table: "Passives");

            migrationBuilder.AddColumn<string>(
                name: "AttributeName",
                table: "Passives",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
