using Microsoft.EntityFrameworkCore.Migrations;

namespace Kindergarten_Management_System.Data.Migrations
{
    public partial class FunSideUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TeacherId",
                table: "FunSides",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FunSides_TeacherId",
                table: "FunSides",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_FunSides_AspNetUsers_TeacherId",
                table: "FunSides",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FunSides_AspNetUsers_TeacherId",
                table: "FunSides");

            migrationBuilder.DropIndex(
                name: "IX_FunSides_TeacherId",
                table: "FunSides");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "FunSides");
        }
    }
}
