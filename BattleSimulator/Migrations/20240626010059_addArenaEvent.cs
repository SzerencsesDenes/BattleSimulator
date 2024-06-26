using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BattleSimulator.Migrations
{
    /// <inheritdoc />
    public partial class addArenaEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Soldier_Arena_ArenaId",
                table: "Soldier");

            migrationBuilder.AlterColumn<int>(
                name: "ArenaId",
                table: "Soldier",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArenaId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Arena_ArenaId",
                        column: x => x.ArenaId,
                        principalTable: "Arena",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_ArenaId",
                table: "Events",
                column: "ArenaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Soldier_Arena_ArenaId",
                table: "Soldier",
                column: "ArenaId",
                principalTable: "Arena",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Soldier_Arena_ArenaId",
                table: "Soldier");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.AlterColumn<int>(
                name: "ArenaId",
                table: "Soldier",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Soldier_Arena_ArenaId",
                table: "Soldier",
                column: "ArenaId",
                principalTable: "Arena",
                principalColumn: "Id");
        }
    }
}
