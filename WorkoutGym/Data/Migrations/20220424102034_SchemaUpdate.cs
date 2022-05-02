using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkoutGym.Data.Migrations
{
    public partial class SchemaUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "WorkoutSession",
                newName: "WorkoutSessionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkoutSessionId",
                table: "WorkoutSession",
                newName: "Id");
        }
    }
}
