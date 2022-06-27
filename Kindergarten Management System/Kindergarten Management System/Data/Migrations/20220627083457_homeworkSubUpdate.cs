using Microsoft.EntityFrameworkCore.Migrations;

namespace Kindergarten_Management_System.Data.Migrations
{
    public partial class homeworkSubUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "HomeWorkSubs");

            migrationBuilder.AddColumn<string>(
                name: "Grade",
                table: "HomeWorkSubs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grade",
                table: "HomeWorkSubs");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "HomeWorkSubs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
