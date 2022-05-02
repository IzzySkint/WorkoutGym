using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkoutGym.Data.Migrations
{
    public partial class SchemaUpdate4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkoutSessionId",
                table: "MemberSession",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "WorkoutSession",
                columns: table => new
                {
                    WorkoutSessionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutSession", x => x.WorkoutSessionId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberSession_WorkoutSessionId",
                table: "MemberSession",
                column: "WorkoutSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_MemberSession_WorkoutSession_WorkoutSessionId",
                table: "MemberSession",
                column: "WorkoutSessionId",
                principalTable: "WorkoutSession",
                principalColumn: "WorkoutSessionId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberSession_WorkoutSession_WorkoutSessionId",
                table: "MemberSession");

            migrationBuilder.DropTable(
                name: "WorkoutSession");

            migrationBuilder.DropIndex(
                name: "IX_MemberSession_WorkoutSessionId",
                table: "MemberSession");

            migrationBuilder.DropColumn(
                name: "WorkoutSessionId",
                table: "MemberSession");
        }
    }
}
