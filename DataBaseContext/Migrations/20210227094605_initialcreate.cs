using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataBaseContext.Migrations
{
    public partial class initialcreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HeroInfoHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TurnNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    GenderId = table.Column<int>(type: "INTEGER", nullable: false),
                    RaceId = table.Column<int>(type: "INTEGER", nullable: false),
                    Health = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxHealth = table.Column<int>(type: "INTEGER", nullable: false),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    Money = table.Column<int>(type: "INTEGER", nullable: false),
                    Alive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Experience = table.Column<int>(type: "INTEGER", nullable: false),
                    ExperienceToLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxBagSize = table.Column<int>(type: "INTEGER", nullable: false),
                    PowerPhysical = table.Column<int>(type: "INTEGER", nullable: false),
                    PowerMagic = table.Column<int>(type: "INTEGER", nullable: false),
                    MoveSpeed = table.Column<double>(type: "REAL", nullable: false),
                    LootItemsCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Initiative = table.Column<double>(type: "REAL", nullable: false),
                    Energy = table.Column<int>(type: "INTEGER", nullable: false),
                    InPvpQueue = table.Column<string>(type: "TEXT", nullable: true),
                    Mode = table.Column<string>(type: "TEXT", nullable: true),
                    Enemy = table.Column<string>(type: "TEXT", nullable: true),
                    Honor = table.Column<double>(type: "REAL", nullable: false),
                    Peacefulness = table.Column<double>(type: "REAL", nullable: false),
                    ActionType = table.Column<int>(type: "INTEGER", nullable: false),
                    ActionDescription = table.Column<string>(type: "TEXT", nullable: true),
                    ActionPercents = table.Column<double>(type: "REAL", nullable: false),
                    ActionIsBoss = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeroInfoHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Turns",
                columns: table => new
                {
                    Number = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VerboseDate = table.Column<string>(type: "TEXT", nullable: true),
                    VerboseTime = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turns", x => x.Number);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HeroInfoHistory");

            migrationBuilder.DropTable(
                name: "Turns");
        }
    }
}
