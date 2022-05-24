using Microsoft.EntityFrameworkCore.Migrations;

namespace Kindergarten_Management_System.Data.Migrations
{
    public partial class updateFunSide : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FunSides_AspNetUsers_TeacherId",
                table: "FunSides");

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

            migrationBuilder.AddForeignKey(
                name: "FK_FunSides_AspNetUsers_TeacherId",
                table: "FunSides",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
