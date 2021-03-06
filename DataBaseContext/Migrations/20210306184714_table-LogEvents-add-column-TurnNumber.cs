using Microsoft.EntityFrameworkCore.Migrations;

namespace DataBaseContext.Migrations
{
    public partial class tableLogEventsaddcolumnTurnNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TurnNumber",
                table: "LogEvents",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TurnNumber",
                table: "LogEvents");
        }
    }
}
